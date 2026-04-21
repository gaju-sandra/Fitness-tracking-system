# FitTrack UI/UX Summary — Quick Reference

## ✅ What's Been Built

### CSS & JavaScript Foundation
- ✅ **`wwwroot/css/fittrack.css`** (600+ lines)
  - Dark/light theme with CSS variables
  - 50+ component classes
  - Responsive grid system (12-column)
  - Animations (float, transitions)
  - Mobile-first design

- ✅ **`wwwroot/js/fittrack.js`** (200+ lines)
  - Theme toggle with localStorage persistence
  - Modal open/close functionality
  - Toast notifications (success/error)
  - Table filtering by text input
  - CSV export from any table
  - Backward-compatible legacy functions

### Razor Pages & Layout
- ✅ **`Pages/_Layout.cshtml`** — Master layout with CSS/JS includes
- ✅ **`Pages/Shared/_Nav.cshtml`** — Responsive navbar with auth & theme toggle
- ✅ **`Pages/Shared/_Footer.cshtml`** — Site footer with links

### Pages Showcasing All Features
- ✅ **`Pages/Index.cshtml`** — Landing page
  - Hero section with gradient text & floating emoji
  - Features grid (6 features)
  - Roles grid (4 roles)
  - CTA section
  - Fully responsive

- ✅ **`Pages/Dashboard.cshtml`** — Main dashboard
  - Welcome header with quick action
  - 4 stat cards (weight, calories, workouts, mood)
  - Goals section with progress bars
  - Badges/achievements display
  - Recent workouts list
  - Today's nutrition log
  - Mood & energy bars
  - AI insights section
  - Quick action buttons

- ✅ **`Pages/Nutrition.cshtml`** — Nutrition tracker
  - Macro stats (calories, protein, carbs, fat)
  - Meal logging by category (breakfast, lunch, dinner, snacks)
  - Local food database reference
  - Modal-based meal addition form
  - Search/filter functionality
  - CSV export
  - Delete meal capability
  - Subtotal calculations

### Documentation
- ✅ **`UI_UX_GUIDE.md`** — Architecture overview & usage guide
- ✅ **`DESIGN_SYSTEM.md`** — Complete component library & design tokens

---

## 🎨 Design System at a Glance

### Color Palette (Dark Mode Default)
```
Primary Background:  #0f1724 (dark blue-black)
Card Background:     #0b1220 (slightly darker)
Text:                #e6eef6 (light blue-white)
Accent (Primary):    #3b82f6 (bright blue)
Success:             #10b981 (green)
Danger:              #ef4444 (red)
Muted (secondary):   #9aa4b2 (gray-blue)
```

### Typography
```
Font Family: Inter (Google Fonts) + system fallbacks
Font Weights: 400 (regular), 600 (medium), 700 (bold), 800 (extra-bold)
Sizes: 0.85rem (small) → 2.8rem (h1)
Line height: 1.2-1.6 (headings), 1.5 (body)
```

### Spacing & Sizing
```
Gap/Padding units: 8px, 10px, 12px, 14px, 16px, 18px, 20px, 24px, 28px
Border radius: 8px (default), 10px (cards), 12px (modals), 999px (pills/badges)
Icon sizes: 44px, 46px (icons), 72px (large avatar)
Button padding: 8px 12px (small), 10px 16px (medium), 12px 20px (large)
```

### Shadows & Effects
```
Main shadow: 0 6px 18px rgba(2,6,23,0.6) [dark mode]
Glass effect: rgba(255,255,255,0.04)
Transitions: 0.18s ease (standard timing)
Gradients: Linear 90-180deg, multiple stops
```

---

## 🎯 Feature Showcase by Page

### Landing Page (Index.cshtml)
Shows:
- ✅ Hero with call-to-action
- ✅ 6 core features
- ✅ 4 user roles
- ✅ Social proof (stats)
- ✅ Secondary CTA

### Dashboard (Dashboard.cshtml)
Shows:
- ✅ Quick stats overview
- ✅ Goal tracking with progress
- ✅ Achievement system
- ✅ Workout history
- ✅ Nutrition logging
- ✅ Mental health tracking (mood/energy)
- ✅ AI insights
- ✅ Quick action buttons

### Nutrition Tracker (Nutrition.cshtml)
Shows:
- ✅ Macro nutrition tracking
- ✅ Meal categorization
- ✅ Local food database
- ✅ Modal-based CRUD (Create in modal, Delete inline)
- ✅ Search/filter
- ✅ Data export (CSV)
- ✅ Subtotal calculations

---

## 📱 Responsive Breakpoints

```
Desktop (1100px+):  Full-width, 4-col grids, 2-col layouts
Tablet (900px):    2-col grids, collapsed layouts, hidden nav
Mobile (720px):    Single-column, stacked elements, mobile nav toggle
```

---

## 🔧 JavaScript Functions (Global Scope)

```javascript
// Theme
toggleTheme()                               // Toggle dark/light, save to localStorage

// Modals
openModal(id)                               // Show modal by ID
closeModal(id)                              // Hide modal by ID

// Notifications
ft.toast(message, type, ms)                // Show toast (type: 'success'|'error', ms: duration)

// Table Operations
filterTable(selector, tableSelector)        // Filter table rows by input text
exportTableToCSV(tableSelector, filename)   // Export table to CSV file

// Data Attributes for JS Hooks
data-action="toggle-theme"                  // Click to toggle theme
data-action="close-modal"                   // Click to close modal
data-target="modalId"                       // Target modal for actions
data-table-filter="#tableId"                // Input auto-filters table
data-csv-export data-target="#tableId"      // Button exports table as CSV
```

---

## 🎨 Component Quick Reference

### Buttons
```html
<button class="btn btn-primary">Primary</button>
<button class="btn btn-ghost">Secondary</button>
<button class="btn btn-success">Success</button>
<button class="btn btn-danger">Danger</button>
<button class="action-btn">Action</button>
```

### Cards
```html
<div class="dash-card">Content</div>
<div class="stat-card"><div class="stat-icon">🎯</div><div>...</div></div>
<div class="goal-card">...</div>
<div class="feature-card">...</div>
<div class="role-card">...</div>
```

### Badges & Labels
```html
<span class="badge">Default</span>
<span class="badge success">Success</span>
<span class="badge warn">Warning</span>
<span class="badge-role">Admin</span>
<span class="goal-type-badge">45%</span>
<span class="meal-badge">Breakfast</span>
```

### Forms
```html
<input class="input" type="text" placeholder="..." />
<label class="label">Label Text</label>
<div class="form-group">...</div>
<div class="form-row"><div class="form-group">...</div></div>
```

### Lists
```html
<div class="card-list">
  <div class="card-item">Item</div>
  <div class="workout-item">Workout</div>
  <div class="nutrition-item">Food</div>
</div>
```

### Progress & Mood
```html
<div class="progress-bar"><div class="progress-fill" style="width: 45%;"></div></div>
<div class="mood-bar"><div class="mood-fill" style="width: 80%;"></div></div>
<div class="mood-bar"><div class="energy-fill" style="width: 100%;"></div></div>
```

### Modals
```html
<div id="myModal">
  <div class="modal-backdrop" onclick="closeModal('myModal')">
    <div class="modal">
      <div class="modal-header">
        <h2 class="modal-title">Title</h2>
        <button class="modal-close" data-action="close-modal" data-target="myModal">&times;</button>
      </div>
      <!-- Content -->
      <div class="modal-footer">
        <button class="btn btn-ghost" data-action="close-modal">Cancel</button>
        <button class="btn btn-primary">Save</button>
      </div>
    </div>
  </div>
</div>
```

---

## 🔗 CSS Grid System

```html
<!-- 12-column system -->
<div class="dashboard-grid">
  <div class="col-3">25%</div>
  <div class="col-4">33%</div>
  <div class="col-6">50%</div>
  <div class="col-8">66%</div>
</div>
```

---

## 📊 Metric Cards (Stats Display)

```html
<div class="stat-card">
  <div class="stat-icon">⚖️</div>
  <div>
    <div class="stat-label">Label</div>
    <div class="stat-value">72.5 kg</div>
    <div class="muted-small">Secondary info</div>
  </div>
</div>
```

---

## 🌓 Theme Implementation

In CSS:
```css
:root { /* dark mode variables */ }
[data-theme="light"] { /* light mode overrides */ }
```

In HTML:
```html
<button data-action="toggle-theme">🌙</button>

<script>
  // Auto-apply saved theme on page load
  applyTheme(currentTheme());
</script>
```

---

## 📄 Form Layout Patterns

### Single Column
```html
<div class="form-group">
  <label class="label">Field</label>
  <input class="input" />
</div>
```

### Two Columns
```html
<div class="form-row">
  <div class="form-group" style="flex: 1;">
    <label class="label">Field 1</label>
    <input class="input" />
  </div>
  <div class="form-group" style="flex: 1;">
    <label class="label">Field 2</label>
    <input class="input" />
  </div>
</div>
```

---

## 🎯 Usage in Razor Pages

### Include CSS & JS
```html
<!-- In _Layout.cshtml -->
<link rel="stylesheet" href="~/css/fittrack.css" />
<script src="~/js/fittrack.js"></script>
```

### Use Components
```html
<!-- Button with theme toggle -->
<button class="btn btn-primary" data-action="toggle-theme">Toggle Theme</button>

<!-- Modal trigger -->
<button class="btn btn-primary" onclick="openModal('myModal')">Open</button>

<!-- Table with filter & export -->
<input class="input" data-table-filter="#myTable" placeholder="Search..." />
<button class="btn btn-ghost" data-csv-export data-target="#myTable">Export</button>
<table id="myTable" class="table">...</table>

<!-- Toast notification -->
<script>
  ft.toast('Saved successfully!', 'success', 3000);
</script>
```

---

## 📋 File Structure

```
fittrack-app/
├── Pages/
│   ├── _Layout.cshtml          ← Master layout
│   ├── Index.cshtml            ← Landing page
│   ├── Dashboard.cshtml        ← Main dashboard
│   ├── Nutrition.cshtml        ← Nutrition tracker
│   └── Shared/
│       ├── _Nav.cshtml         ← Navigation partial
│       └── _Footer.cshtml      ← Footer partial
├── wwwroot/
│   ├── css/
│   │   └── fittrack.css        ← All styles + components
│   └── js/
│       └── fittrack.js         ← All JS functionality
├── Data/
│   └── AppDbContext.cs         ← EF Core models
├── Models/
│   └── [Domain models]
├── UI_UX_GUIDE.md              ← Architecture guide
└── DESIGN_SYSTEM.md            ← Component library
```

---

## 🚀 Next Steps to Complete UI

1. **Create CRUD Pages** (Use same CSS grid & component patterns)
   - Workouts/Index.cshtml, Workouts/Create.cshtml, Workouts/Edit.cshtml
   - Goals/Index.cshtml, Goals/Create.cshtml, Goals/Edit.cshtml
   - BodyWeights/Index.cshtml
   - MoodLogs/Index.cshtml

2. **Create Admin Pages**
   - Admin/Users/Index.cshtml (user management table)
   - Admin/Foods/Index.cshtml (food database)
   - Admin/WorkoutPlans/Index.cshtml

3. **Create Trainer Pages**
   - Trainer/Clients/Index.cshtml (client list + dashboard views)

4. **Enhance Dashboard**
   - Add charts (weight trend, calorie burn, macro breakdown)
   - Add notifications panel
   - Add achievement unlock animations

5. **Polish & Optimize**
   - Add loading states
   - Add error handling UI
   - Add confirmation modals for destructive actions
   - Add pagination for large tables
   - Add dark/light mode icons/indicators

---

## 💡 Design Tips

✅ Always use semantic HTML (`<button>` not `<div onclick>`)
✅ Use CSS variables for colors—makes theme switching easy
✅ Keep grid system consistent (12-column)
✅ Use 12px gap throughout for consistency
✅ Every button should be at least 44px tall (mobile friendly)
✅ Every interactive element needs a focus state
✅ Test on real devices, not just browser dev tools
✅ Use animations sparingly—0.18s is the standard timing

---

**Last Updated**: January 2025  
**Version**: 1.0  
**Status**: Complete landing page, dashboard, & nutrition tracker with all CSS/JS foundations

