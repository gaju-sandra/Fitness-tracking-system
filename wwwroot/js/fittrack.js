// fittrack.js - modal, toast, theme toggle, table filter, CSV export
(function(){
  // Theme toggle
  const root = document.documentElement;
  const themeKey = 'fittrack:theme';
  const themeIcon = () => document.querySelector('[data-theme-toggle-icon]');
  function applyTheme(t){
    if(t === 'dark') root.setAttribute('data-theme','dark');
    else root.setAttribute('data-theme','light');
    const icon = themeIcon();
    if(icon) icon.textContent = t === 'dark' ? '☀️' : '🌙';
  }
  function currentTheme(){ return localStorage.getItem(themeKey) || 'light'; }
  applyTheme(currentTheme());
  document.addEventListener('click', e => {
    const t = e.target.closest('[data-action="toggle-theme"]');
    if(!t) return;
    const next = currentTheme() === 'light' ? 'dark' : 'light';
    localStorage.setItem(themeKey,next);
    applyTheme(next);
  });

  // Simple modal
  window.ft = window.ft || {};
  ft.openModal = function(id){
    const el = document.getElementById(id);
    if(!el) return;
    el.classList.add('open');
    el.querySelector('.modal-backdrop')?.classList.add('open');
    document.body.style.overflow = 'hidden';
  }
  ft.closeModal = function(id){
    const el = document.getElementById(id);
    if(!el) return;
    el.classList.remove('open');
    el.querySelector('.modal-backdrop')?.classList.remove('open');
    if(!document.querySelector('.modal-overlay.open, .modal-backdrop.open')) {
      document.body.style.overflow = '';
    }
  }
  // Backwards-compatible global functions expected by pages
  window.openModal = function(id){ return ft.openModal(id); };
  window.closeModal = function(id){ return ft.closeModal(id); };
  document.addEventListener('click', e => {
    const cb = e.target.closest('[data-action="close-modal"]');
    if(cb){
      const id = cb.getAttribute('data-target');
      if(id) ft.closeModal(id);
      else {
        const modal = cb.closest('.modal');
        if(modal) ft.closeModal(modal.id);
      }
    }

    const overlay = e.target.closest('.modal-overlay.open');
    if(overlay && e.target === overlay){
      ft.closeModal(overlay.id);
    }
  });

  document.addEventListener('keydown', e => {
    if(e.key !== 'Escape') return;
    const openModal = document.querySelector('.modal-overlay.open');
    if(openModal) ft.closeModal(openModal.id);
  });

  // Toasts
  const toastContainer = document.createElement('div');
  toastContainer.className = 'toast-container';
  document.body.appendChild(toastContainer);
  ft.toast = function(message, type='default', ms=4000){
    const t = document.createElement('div');
    t.className = 'toast '+(type==='success'? 'success' : type==='error' ? 'error' : '');
    t.innerHTML = `<div class="msg">${escapeHtml(message)}</div><button class="modal-close" aria-label="close">&times;</button>`;
    t.querySelector('.modal-close').addEventListener('click', ()=> t.remove());
    toastContainer.appendChild(t);
    setTimeout(()=>{ t.remove(); }, ms);
  }

  function escapeHtml(s){ return String(s).replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;'); }

  // Table filter (by text input) - data-filter on table wraps
  document.addEventListener('input', e =>{
    const inp = e.target.closest('[data-table-filter]');
    if(!inp) return;
    const selector = inp.getAttribute('data-table-filter');
    const q = inp.value.trim().toLowerCase();
    const table = document.querySelector(selector);
    if(!table) return;
    const rows = Array.from(table.querySelectorAll('tbody tr'));
    rows.forEach(r => {
      const text = r.textContent.trim().toLowerCase();
      r.style.display = text.indexOf(q) === -1 ? 'none' : ''; 
    });
  });

  // Legacy function: filterTable(inputOrSelector, tableSelector?)
  window.filterTable = function(inputOrSelector, tableSelector){
    let inputEl;
    if(typeof inputOrSelector === 'string') inputEl = document.querySelector(inputOrSelector);
    else inputEl = inputOrSelector;
    if(!inputEl) return;
    // If tableSelector not provided, try data attribute
    const selector = tableSelector || inputEl.getAttribute('data-table-filter');
    if(!selector) return;
    const q = (inputEl.value || '').trim().toLowerCase();
    const table = document.querySelector(selector);
    if(!table) return;
    const rows = Array.from(table.querySelectorAll('tbody tr'));
    rows.forEach(r => {
      const text = r.textContent.trim().toLowerCase();
      r.style.display = text.indexOf(q) === -1 ? 'none' : '';
    });
  };

  // CSV Export - data-csv-export on button -> data-target table selector
  document.addEventListener('click', e=>{
    const btn = e.target.closest('[data-csv-export]');
    if(!btn) return;
    const sel = btn.getAttribute('data-target');
    if(!sel) return;
    const table = document.querySelector(sel);
    if(!table) return;
    const csv = tableToCsv(table);
    downloadBlob(csv, `export-${Date.now()}.csv`, 'text/csv;charset=utf-8;');
  });

  // Legacy export function: exportTableToCSV(tableSelector, filename)
  window.exportTableToCSV = function(tableSelector, filename){
    const table = document.querySelector(tableSelector);
    if(!table) return;
    const csv = tableToCsv(table);
    const name = filename || `export-${Date.now()}.csv`;
    downloadBlob(csv, name, 'text/csv;charset=utf-8;');
  };

  // Legacy theme toggle function
  window.toggleTheme = function(){
    const key = themeKey;
    const cur = localStorage.getItem(key) || (window.matchMedia && window.matchMedia('(prefers-color-scheme: light)').matches ? 'light' : 'dark');
    const next = cur === 'light' ? 'dark' : 'light';
    localStorage.setItem(key, next);
    applyTheme(next);
  };

  window.toggleNav = function(){
    const nav = document.getElementById('navLinks');
    if(!nav) return;
    nav.classList.toggle('open');
  };

  function tableToCsv(table){
    const rows = [];
    const headers = Array.from(table.querySelectorAll('thead th')).map(h => '"'+h.textContent.trim().replace(/"/g,'""')+'"');
    if(headers.length) rows.push(headers.join(','));
    const trs = table.querySelectorAll('tbody tr');
    trs.forEach(tr =>{
      const cols = Array.from(tr.querySelectorAll('td')).map(td => '"'+td.textContent.trim().replace(/"/g,'""')+'"');
      rows.push(cols.join(','));
    });
    return rows.join('\n');
  }

  function downloadBlob(text, filename, mime){
    const blob = new Blob([text], {type:mime});
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url; a.download = filename; document.body.appendChild(a); a.click(); a.remove();
    setTimeout(()=>URL.revokeObjectURL(url), 5000);
  }

})();
