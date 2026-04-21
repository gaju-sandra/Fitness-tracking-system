# 🎨 FitTrack UI/UX Design System & Component Guide

## Project Overview

**FitTrack** is a comprehensive fitness tracking application built with:
- **Framework**: ASP.NET Core Razor Pages (.NET 10)
- **Database**: Entity Framework Core with SQL Server
- **Frontend**: Custom CSS (dark/light theme) + Vanilla JavaScript
- **Target Users**: Fitness enthusiasts, trainers, admins, and everyone

---

## 📋 Page Architecture

### 1. **Landing Page** (`Pages/Index.cshtml`)
**Purpose**: Homepage showcasing FitTrack features and benefits

**Key Sections**:
```
┌─────────────────────────────────────────────┐
│         HERO SECTION (1:1.3 ratio)          │
│  ┌──────────────────────┬──────────────┐   │
│  │ Hero Content         │  Hero Visual │   │
│  │ - Badge              │  (💪 emoji) │   │
│  │ - Gradient Title     │  Floating   │   │
│  │ - Description        │  animation  │   │
│  │ - CTAs (2 buttons)   │             │   │
│  │ - Stats (3 columns)  │             │   │
│  └──────────────────────┴──────────────┘   │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│      FEATURES SECTION (3-column grid)       │
│  ┌──────────┬──────────┬──────────┐        │
│  │ ⚖️ Weight│ 🍎 Nutr. │ 🏋️ Work │        │
│  │ Tracking │ Logging  │ out      │        │
│  │ Desc     │ Desc     │ Desc     │        │
│  └──────────┴──────────┴──────────┘        │
│  ┌──────────┬──────────┬──────────┐        │
│  │ 🎯 Goals │ 😊 Mood  │ 🏆 Achieve │     │
│  └──────────┴──────────┴──────────┘        │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│      ROLES SECTION (4-column grid)          │
│  ┌────────┬────────┬────────┬────────┐    │
│  │👤 Users│👨 Train│⚙️ Admin│🌍 All │    │
│  │        │ers     │        │        │    │
│  └────────┴────────┴────────┴────────┘    │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│         CTA SECTION (Centered)              │
│    [Primary Button] - Start Account         │
└─────────────────────────────────────────────┘
```

**Design Tokens Used**:
- Hero: `.hero`, `.hero-content`, `.hero-visual`, `.gradient-text`
- Features: `.features-grid`, `.feature-card`, `.feature-icon`
- Roles: `.roles-grid`, `.role-card`
- CTA: `.cta-section`

**Key Features**:
✅ Responsive hero layout (2-col → 1-col on mobile)
✅ Gradient text effect on main headline
✅ Floating emoji animation
✅ Dark/light theme support
✅ Mobile-optimized buttons

---

### 2. **Dashboard** (`Pages/Dashboard.cshtml`)
**Purpose**: Main user interface showing fitness overview and quick actions

**Key Sections**:
```
┌──────────────────────────────────────────────┐
│  Dashboard Header: "Welcome back, [User]!" │ [+ Log Workout] │
└──────────────────────────────────────────────┘

┌─────────────┬──────────────┬────────┬─────────┐
│ ⚖️ Weight   │ 🔥 Calories  │ 💪 Work│ 😊 Mood │
│ 72.5 kg     │ 1,850 kcal   │outs: 4 │ 4/5     │
│ ↓ 2.0kg     │ 1,500 goal   │ 1 more │ Great!  │
└─────────────┴──────────────┴────────┴─────────┘

┌──────────────────────────────┬──────────────┐
│     🎯 YOUR GOALS (2:1)      │  🏆 BADGES  │
│ ┌──────────────────────────┐ │ ┌──────────┐│
│ │ Lose 5kg                 │ │ │ 🏋️ 🔥 🥗││
│ │ ████░░░░ 45% • Aug 1     │ │ │ ⚖️ 🎉 📈 │
│ │ 70.5kg → 67.5kg          │ │ │ 4/12     │
│ │                          │ │ │          │
│ │ Run 5km Daily            │ │ └──────────┘
│ │ ██████░░ 60% • Jul 1     │ │
│ │ 3km → 5km                │ │
│ └──────────────────────────┘ │
└──────────────────────────────┴──────────────┘

┌──────────────────────────────┬──────────────┐
│   💪 RECENT WORKOUTS (1:1)   │ 🍎 NUTRITION│
│ ┌──────────────────────────┐ │ ┌──────────┐│
│ │ Morning Cardio           │ │ │Breakfast │
│ │ 35 min • 280 kcal ✓      │ │ │ 150kcal  │
│ │                          │ │ │          │
│ │ Strength Builder         │ │ │Lunch     │
│ │ 45 min • 320 kcal ✓      │ │ │ 694kcal  │
│ │                          │ │ │          │
│ │ HIIT Blast               │ │ │Dinner    │
│ │ 30 min • 250 kcal ✓      │ │ │ 247kcal  │
│ └──────────────────────────┘ │ └──────────┘
└──────────────────────────────┴──────────────┘

┌──────────────────────────────┬──────────────┐
│    😊 MOOD TODAY (1:1)       │ 🤖 INSIGHTS  │
│ ┌──────────────────────────┐ │ ┌──────────┐│
│ │Mood: ████████░░░ 4/5    │ │ │Great!    │
│ │Energy: ██████████ 5/5   │ │ │Keep +1   │
│ └──────────────────────────┘ │ │workout   │
│                              │ │          │
│                              │ │💡Eat more│
│                              │ │protein   │
│                              │ └──────────┘
└──────────────────────────────┴──────────────┘

┌──────────────────────────────────────────────┐
│ Quick Actions:                               │
│ [+ Workout] [+ Meal] [+ Goal] [+ Mood] [+ Weight]
└──────────────────────────────────────────────┘
```

**Design Tokens Used**:
- Header: `.dashboard-header`
- Stats: `.stats-grid`, `.stat-card`, `.stat-icon`, `.stat-value`
- Grid: `.dashboard-grid`, `.col-4`, `.col-6`, `.col-8`
- Goals: `.goals-list`, `.goal-card`, `.progress-bar`, `.progress-fill`
- Badges: `.badges-grid`, `.badge-item`
- Mood: `.mood-display`, `.mood-bar`, `.mood-fill`, `.energy-fill`
- Cards: `.dash-card`
- Actions: `.action-btn`

**Key Features**:
✅ At-a-glance stats with icons
✅ Progress bars for goals
✅ Visual mood/energy indicators
✅ Recent activity cards
✅ Quick action buttons
✅ AI insights
✅ Responsive grid (4-col → 2-col → 1-col)

---

### 3. **Nutrition Tracker** (`Pages/Nutrition.cshtml`)
**Purpose**: Log meals, track nutrition, view macros

**Key Sections**:
```
┌──────────────────────────────────────────────┐
│ 🍎 Nutrition Tracker      [+ Add Meal]      │
└──────────────────────────────────────────────┘

┌──────┬──────┬──────┬──────┐
│ 🔥1850│ 🥚58g│ 🌾258g│ 🧈42g│
│kcal │Protein│Carbs │ Fat  │
│+350 │48%   │143%  │84%   │
└──────┴──────┴──────┴──────┘

[Search meals...] [📊 Export]

┌──────────────────────────┬──────────────────┐
│  🌅 BREAKFAST (1:1)      │ 🌞 LUNCH (1:1)   │
│ ┌──────────────────────┐│ ┌────────────────┐│
│ │Banana (Igitoki)      ││ │Beans (Ishi.)  ││
│ │150g•133.5kcal [Delete]││ │200g•694kcal[Del]││
│ │                      ││ │                ││
│ │Milk (Amata)          ││ │ Subtotal:     ││
│ │100ml•61kcal [Delete] ││ │ 694 kcal      ││
│ │                      ││ │                ││
│ │ Subtotal: 194.5kcal  ││ └────────────────┘
│ └──────────────────────┘│
└──────────────────────────┴──────────────────┘

┌──────────────────────────┬──────────────────┐
│  🌙 DINNER (1:1)         │ 🇷🇼 LOCAL FOODS    │
│ ┌──────────────────────┐│ ┌────────────────┐│
│ │Chicken Breast        ││ │Ugali 360kcal   ││
│ │150g•247.5kcal [Delete]││ │Isombe 45kcal   ││
│ │                      ││ │Potatoes 77kcal ││
│ │ Subtotal: 247.5kcal  ││ │Beans 347kcal   ││
│ └──────────────────────┘│ └────────────────┘
└──────────────────────────┴──────────────────┘
```

**Modal (Add Meal)**:
```
╔══════════════════════════════╗
║  Add Meal              [X]   ║
╠══════════════════════════════╣
║ Meal Type:                   ║
║ [Breakfast ▼]                ║
║                              ║
║ Food Item:                   ║
║ [Search or enter...]         ║
║                              ║
║ Amount:    Unit:             ║
║ [100]      [g ▼]             ║
║                              ║
║ Time:                        ║
║ [14:30]                      ║
║                              ║
║ Notes:                       ║
║ ┌────────────────────────┐  ║
║ │                        │  ║
║ └────────────────────────┘  ║
║                              ║
║ [Cancel]          [Save Meal]║
╚══════════════════════════════╝
```

**Design Tokens Used**:
- Header: `.page-header`
- Stats: `.summary-stats`, `.stat-card`
- Filter: `.filter-bar`, `.input`
- Grid: `.dashboard-grid`, `.col-6`
- Cards: `.dash-card`, `.nutrition-item`, `.card-list`
- Modal: `.modal-backdrop`, `.modal`, `.form-group`, `.form-row`
- Badges: `.meal-badge`

**Key Features**:
✅ Macro tracking (protein, carbs, fat)
✅ Meal categorization (breakfast, lunch, dinner, snacks)
✅ Local food database
✅ Modal-based meal addition
✅ Delete functionality
✅ CSV export
✅ Subtotal calculations
✅ Search/filter

---

## 🎨 Component Library

### Buttons
```html
<!-- Primary (CTA) -->
<button class="btn btn-primary">Get Started Free</button>

<!-- Ghost (Secondary) -->
<button class="btn btn-ghost">Sign In</button>

<!-- Danger -->
<button class="btn btn-danger">Delete</button>

<!-- Success -->
<button class="btn btn-success">Save</button>

<!-- Action -->
<button class="action-btn">+ Log Workout</button>
```

### Cards
```html
<!-- Stat Card -->
<div class="stat-card">
  <div class="stat-icon">⚖️</div>
  <div>
    <div class="stat-label">Current Weight</div>
    <div class="stat-value">72.5 kg</div>
    <div class="muted-small">↓ 2.0 kg from start</div>
  </div>
</div>

<!-- Dash Card -->
<div class="dash-card">
  <h2>Title</h2>
  <!-- Content -->
</div>

<!-- Goal Card -->
<div class="goal-card">
  <div class="goal-card-header">
    <div>
      <div style="font-weight: 700;">Lose 5kg</div>
      <div class="goal-meta">Weight • Due Aug 1</div>
    </div>
    <span class="goal-type-badge">45%</span>
  </div>
  <div class="progress-bar">
    <div class="progress-fill" style="width: 45%;"></div>
  </div>
</div>
```

### Forms
```html
<div class="form-group">
  <label class="label">Label</label>
  <input type="text" class="input" placeholder="..." />
</div>

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

### Badges & Pills
```html
<!-- Badge -->
<span class="badge">Default</span>
<span class="badge success">Success</span>
<span class="badge warn">Warning</span>

<!-- Role Badge -->
<span class="badge-role">Admin</span>

<!-- Meal Badge -->
<span class="meal-badge">Breakfast</span>
```

### Progress Bars
```html
<!-- Goal Progress -->
<div class="progress-bar">
  <div class="progress-fill" style="width: 45%;"></div>
</div>

<!-- Mood Bar -->
<div class="mood-bar">
  <div class="mood-fill" style="width: 80%;"></div>
</div>

<!-- Energy Bar -->
<div class="mood-bar">
  <div class="energy-fill" style="width: 100%;"></div>
</div>
```

### Lists
```html
<div class="card-list">
  <div class="card-item">Item content</div>
  <div class="card-item">Item content</div>
</div>

<div class="card-list">
  <div class="workout-item">Workout item</div>
  <div class="nutrition-item">Nutrition item</div>
</div>
```

### Modals
```html
<div id="myModal" style="display: none;">
  <div class="modal-backdrop" onclick="closeModal('myModal')">
    <div class="modal" onclick="event.stopPropagation()">
      <div class="modal-header">
        <h2 class="modal-title">Modal Title</h2>
        <button class="modal-close" data-action="close-modal" data-target="myModal">&times;</button>
      </div>
      
      <!-- Content -->
      
      <div class="modal-footer">
        <button class="btn btn-ghost" data-action="close-modal" data-target="myModal">Cancel</button>
        <button class="btn btn-primary">Save</button>
      </div>
    </div>
  </div>
</div>
```

---

## 🌓 Theme System

### Dark Mode (Default)
- Background: Linear gradient from `#0f1724` to darker
- Cards: `#0b1220`
- Text: `#e6eef6`
- Accent: `#3b82f6` (Blue)
- Success: `#10b981` (Green)
- Danger: `#ef4444` (Red)
- Glass: `rgba(255,255,255,0.04)`

### Light Mode
- Background: Linear gradient from `#f6f8fb`
- Cards: `#ffffff`
- Text: `#0b1320`
- Accent: `#2563eb` (Darker Blue)
- Success: `#059669` (Darker Green)
- Danger: `#dc2626` (Darker Red)
- Glass: `rgba(10,15,25,0.03)`

**Toggle**: Add `data-action="toggle-theme"` to any button or call `toggleTheme()`

---

## 📱 Responsive Behavior

| Screen | Hero | Stats | Goals | Nutrition |
|--------|------|-------|-------|-----------|
| Desktop (1100px) | 2-col | 4-col | 2/3-1/3 | 1/2-1/2 |
| Tablet (900px) | 1-col | 2-col | 1-col | 1-col |
| Mobile (720px) | 1-col | 1-col | 1-col | 1-col |

---

## 🎬 Animations

```css
/* Float (Hero emoji) */
@keyframes float {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-12px); }
}

/* Transitions */
- Modal backdrop: 0.18s ease
- Focus outlines: Smooth 0.18s ease
- Button hover: Smooth transitions
```

---

## ♿ Accessibility Features

✅ Proper ARIA labels on modals
✅ Focus states with outlines
✅ Color contrast ratios meet WCAG AA
✅ Semantic HTML (buttons, labels, etc.)
✅ Keyboard navigation support
✅ Theme respects system preferences
✅ Touch-friendly button sizes (min 44px)

---

## 📊 Data Visualization (Future)

Planned chart integrations:
- Weight trend line graph
- Calorie burn chart
- Macro pie chart
- Goal progress timeline
- Workout frequency calendar
- Mood trend line

---

## 🚀 Next Implementation Steps

1. ✅ **UI Layout** (Done - Index, Dashboard, Nutrition)
2. ⏳ **CRUD Pages** (Workouts, Goals, BodyWeights, MoodLogs)
3. ⏳ **Admin Pages** (Users, Foods, Plans, Reports)
4. ⏳ **Trainer Pages** (Clients, Assignments)
5. ⏳ **Charts & Analytics** (Chart.js or similar)
6. ⏳ **Real-time Features** (SignalR for notifications)
7. ⏳ **API Integration** (Workout plans, food DB)
8. ⏳ **Performance** (Caching, pagination, lazy loading)

---

## 📚 Files Created

| File | Purpose |
|------|---------|
| `wwwroot/css/fittrack.css` | Complete design system + components |
| `wwwroot/js/fittrack.js` | Theme toggle, modals, toasts, filters, exports |
| `Pages/_Layout.cshtml` | Master layout wrapper |
| `Pages/Shared/_Nav.cshtml` | Navigation bar partial |
| `Pages/Shared/_Footer.cshtml` | Footer partial |
| `Pages/Index.cshtml` | Landing page |
| `Pages/Dashboard.cshtml` | Main user dashboard |
| `Pages/Nutrition.cshtml` | Nutrition tracker |
| `UI_UX_GUIDE.md` | This guide |

---

## 💡 Pro Tips

1. **Use Data Attributes**: Leverage `data-action`, `data-target`, `data-table-filter` for JS interactivity
2. **Theme Variables**: Always use CSS custom properties (`--bg`, `--text`, `--accent`) for consistency
3. **Responsive Design**: Test at 320px, 720px, 900px, and 1400px breakpoints
4. **Accessibility**: Every interactive element should be keyboard accessible
5. **Performance**: Use CSS Grid/Flexbox instead of CSS-in-JS for animations
6. **Consistency**: Match spacing, radius, shadows across all pages
7. **Mobile First**: Design for mobile, then enhance for desktop

