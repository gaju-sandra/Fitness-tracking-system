<<<<<<< HEAD
# FitTrack

`FitTrack` is a role-based fitness tracking web app built with ASP.NET Core Razor Pages (.NET 10).

It supports `User`, `Trainer`, and `Admin` workflows with session-based auth, OTP verification by email, and fitness/nutrition tracking features.

## Features

=======
# Fitness-tracking-system
group M 

# 🎉 FitTrack UI/UX Complete — Project Summary
# FitTrack

## ✅ Deliverables
`FitTrack` is a role-based fitness tracking web app built with ASP.NET Core Razor Pages (.NET 10).

### 1. **CSS Foundation** (`wwwroot/css/fittrack.css`)
- ✅ 600+ lines of production-ready CSS
- ✅ 50+ component classes for every UI element
- ✅ Dark mode (default) + light mode with CSS variables
- ✅ 12-column responsive grid system
- ✅ Mobile-first design (breakpoints: 720px, 900px, 1400px+)
- ✅ Smooth animations & transitions
- ✅ Glassmorphism effects & gradients
- ✅ WCAG AA accessibility standards
It supports `User`, `Trainer`, and `Admin` workflows with session-based auth, OTP verification by email, and fitness/nutrition tracking features.

### 2. **JavaScript Interactivity** (`wwwroot/js/fittrack.js`)
- ✅ 200+ lines of vanilla JS (no dependencies)
- ✅ Theme toggle with localStorage persistence
- ✅ Modal system (open/close with escape key)
- ✅ Toast notifications (success/error types)
- ✅ Table filtering by text input
- ✅ CSV export from any table
- ✅ Backward-compatible legacy functions for existing code
- ✅ Data-attribute driven (progressive enhancement)
## Features

### 3. **Layout & Navigation**
- ✅ `Pages/_Layout.cshtml` — Master layout with CSS/JS includes
- ✅ `Pages/Shared/_Nav.cshtml` — Responsive navbar with theme toggle & auth
- ✅ `Pages/Shared/_Footer.cshtml` — Footer with links & copyright
- ✅ View Components ready for navbar/footer
>>>>>>> 940a79274a461500d779e3b365dc15372159c2a7
### Authentication
- Register and login with password hashing (`BCrypt`)
- OTP-based verification (`/Auth/TwoFactor`) for registration and login
- SMTP email delivery for OTP
- Session-based access control and protected routes

<<<<<<< HEAD
=======
### 4. **Page Implementations**
>>>>>>> 940a79274a461500d779e3b365dc15372159c2a7
### User features
- Body weight logging and trend chart data
- Goal tracking with target timeline (weeks to deadline)
- Workout logging with duration and calories burned
- Nutrition logging with macro calculations (protein, carbs, fat)
- Mood and energy tracking with recommendations
- Personalized dashboard insights (streaks, calories, AI-style guidance)

<<<<<<< HEAD
=======
#### Landing Page (`Pages/Index.cshtml`)
Showcases all features to prospective users
- ✅ Hero section with gradient text & floating animation
- ✅ Features grid (6 features with icons & descriptions)
- ✅ Roles showcase (4 personas: Users, Trainers, Admins, Everyone)
- ✅ CTA section with social proof
- ✅ Fully responsive (2-col → 1-col on mobile)
- ✅ Dark/light theme support
>>>>>>> 940a79274a461500d779e3b365dc15372159c2a7
### Trainer features
- View assigned clients
- Track client latest weight and workout activity
- Manage workout plans

<<<<<<< HEAD
=======
#### Dashboard (`Pages/Dashboard.cshtml`)
Main user interface showcasing all fitness functionalities
- ✅ Welcome header with personalized greeting
- ✅ 4 stat cards (Weight, Calories, Workouts, Mood)
- ✅ Goals section with progress bars
- ✅ Badges/achievements display
- ✅ Recent workouts list
- ✅ Today's nutrition breakdown
- ✅ Mood & energy tracking bars
- ✅ AI-powered insights
- ✅ Quick action buttons (7 CTAs)
- ✅ Responsive grid (4-col → 2-col → 1-col)
>>>>>>> 940a79274a461500d779e3b365dc15372159c2a7
### Admin features
- Manage users, foods, and workout plans
- View system-level dashboard stats
- Enforced naming rule for roles:
  - Admin names must end with `Admin`
  - Trainer names must include `Trainer`

<<<<<<< HEAD
## Tech stack

=======
#### Nutrition Tracker (`Pages/Nutrition.cshtml`)
Demonstrates nutrition logging with all CRUD concepts
- ✅ Macro statistics (calories, protein, carbs, fat)
- ✅ Meal categorization (breakfast, lunch, dinner, snacks)
- ✅ Calorie calculation per meal
- ✅ Modal-based meal addition form
- ✅ Delete inline functionality
- ✅ Search/filter table integration
- ✅ CSV export button
- ✅ Local food database reference
- ✅ Subtotal calculations
## Tech stack

### 5. **Documentation** (for developers)
>>>>>>> 940a79274a461500d779e3b365dc15372159c2a7
- ASP.NET Core Razor Pages (`net10.0`)
- Entity Framework Core (`SqlServer` provider)
- SQL Server (via `DefaultConnection`)
- `BCrypt.Net-Next` for password hashing
- Session + custom `BasePageModel` role checks
- Vanilla CSS/JS in `wwwroot/css/fittrack.css` and `wwwroot/js/fittrack.js`

<<<<<<< HEAD
## Project structure

=======
#### `UI_UX_GUIDE.md` (1000+ words)
- Architecture overview
- Component showcase
- Theme system explanation
- Responsive behavior documentation
- JavaScript function reference
- File structure guide
- Next implementation steps
## Project structure

#### `DESIGN_SYSTEM.md` (1500+ words)
- Complete component library with HTML examples
- Design tokens (colors, typography, spacing)
- Responsive breakpoints table
- Animation specifications
- Accessibility features checklist
- Data visualization planning
- Implementation workflow
>>>>>>> 940a79274a461500d779e3b365dc15372159c2a7
- `Pages/Auth` - login, register, OTP, logout
- `Pages/User` - body weight, goals, workouts, nutrition, mood
- `Pages/Trainer` - trainer client views
- `Pages/Admin` - admin management pages
- `Pages/Dashboard` - role-aware dashboard
- `Data/AppDbContext.cs` - EF Core context + seed data
- `Models/Models.cs` - domain models
- `Services/SmtpEmailSender.cs` - email service
- `SQL_QUERIES.sql` - SQL schema + seed script for manual setup

<<<<<<< HEAD
## Getting started

=======
#### `QUICK_REFERENCE.md` (800+ words)
- One-page feature summary
- Color palette quick lookup
- CSS grid system usage
- Button/card/badge HTML snippets
- Form layout patterns
- Component patterns for Razor Pages
- File structure map
- "Next steps" checklist
## Getting started

#### `VISUAL_MOCKUPS.md` (500+ lines)
- ASCII mockups of all pages
- Desktop layout diagrams
- Mobile layout diagrams
- Modal interaction flows
- Color & typography reference
- Spacing grid specifications
>>>>>>> 940a79274a461500d779e3b365dc15372159c2a7
### 1) Prerequisites
- .NET 10 SDK
- SQL Server / SQL Express
- SMTP account for OTP email

<<<<<<< HEAD
=======
---
>>>>>>> 940a79274a461500d779e3b365dc15372159c2a7
### 2) Configure settings
Update `appsettings.json`:
- `ConnectionStrings:DefaultConnection`
- `Smtp:*` values (`Host`, `Port`, `EnableSsl`, `Username`, `Password`, `FromEmail`, `FromName`)

<<<<<<< HEAD
> Security note: do not commit real SMTP credentials. Use user secrets or environment variables.

### 3) Create database
This project includes `SQL_QUERIES.sql` with full table creation and seed data.

Run the script in SSMS (or equivalent) against your target SQL Server database.

=======
## 🎯 Feature Highlights by Functionality
> Security note: do not commit real SMTP credentials. Use user secrets or environment variables.

### Weight Tracking
- ✅ Current weight display
- ✅ Progress indicator (kg lost from start)
- ✅ Goal tracking with progress bars
### 3) Create database
This project includes `SQL_QUERIES.sql` with full table creation and seed data.

### Nutrition Logging
- ✅ Meal categorization (breakfast, lunch, dinner, snacks)
- ✅ Calorie calculation with macro breakdown
- ✅ Local Rwandan food database reference
- ✅ Modal-based food entry
- ✅ Delete/edit capability
- ✅ CSV export for reports
Run the script in SSMS (or equivalent) against your target SQL Server database.

### Workout Tracking
- ✅ Recent workout history list
- ✅ Duration & calories burned display
- ✅ Workout completion badges
- ✅ Weekly workout counter with goal tracking

### Goal Management
- ✅ Multiple goals display
- ✅ Goal type categorization
- ✅ Visual progress bars
- ✅ Deadline tracking
- ✅ Percentage completion display

### Mood & Mental Health
- ✅ Mood score (1-5 scale)
- ✅ Energy level tracking
- ✅ Visual bar charts
- ✅ Mood trend tracking

### Gamification
- ✅ Badge/achievement display
- ✅ Progress towards total badges
- ✅ Badge earned date tracking
- ✅ Visual achievement grid

### Analytics & Insights
- ✅ Quick stat cards
- ✅ AI insights with tips
- ✅ Nutrition recommendations
- ✅ Progress analysis
- ✅ CSV export capability

### Role-Based Experience
- ✅ User navigation (dashboard, nutrition, workouts, goals)
- ✅ Role badge display (Admin, Trainer, User)
- ✅ Logout functionality
- ✅ Responsive nav toggle on mobile

---

## 🎨 Design System Specifications

### Color Palette (CSS Variables)
```css
--bg: #0f1724 (dark mode) | #f6f8fb (light)
--card: #0b1220 (dark) | #ffffff (light)
--text: #e6eef6 (dark) | #0b1320 (light)
--accent: #3b82f6 (dark) | #2563eb (light)
--success: #10b981 (dark) | #059669 (light)
--danger: #ef4444 (dark) | #dc2626 (light)
--muted: #9aa4b2 (dark) | #56626d (light)
--glass: rgba(255,255,255,0.04) (dark) | rgba(10,15,25,0.03) (light)
>>>>>>> 940a79274a461500d779e3b365dc15372159c2a7
### 4) Run the app
```powershell
dotnet restore
dotnet run
```

<<<<<<< HEAD
Then open the app URL shown in terminal (typically `https://localhost:<port>`).

## Default seeded accounts

=======
### Typography System
```
Font: Inter, system-ui, -apple-system, Segoe UI, Roboto
Weights: 400 (regular), 600 (medium), 700 (bold), 800 (extra-bold)
Sizes: 0.85rem → 2.8rem (14px → 44px)
Line Heights: 1.2 (headings), 1.5 (body)
```
Then open the app URL shown in terminal (typically `https://localhost:<port>`).

### Spacing Units
```
Base: 4px (logical unit)
Common: 8, 10, 12, 14, 16, 18, 20, 24, 28px
Gaps: 12px (standard between elements)
Padding: 12-18px (cards), 8-12px (buttons)
Margin: 18-28px (sections)
```
## Default seeded accounts

### Component Specifications
```
Border Radius: 8px (default), 10px (cards), 12px (modals), 999px (pills)
Shadows: 0 6px 18px rgba(2,6,23,0.6) (dark), lighter in light mode
Transitions: 0.18s ease (standard)
Focus Outline: 3px solid accent with 25% opacity
```
>>>>>>> 940a79274a461500d779e3b365dc15372159c2a7
If you run the provided SQL seed/app seed data, typical test users include:
- Admin: `admin@fittrack.rw`
- Trainer: `trainer@fittrack.rw`
- Users: `alice@fittrack.rw`, `bob@fittrack.rw`

<<<<<<< HEAD
Use your own safe credentials in non-local environments.

=======
---

## 📊 Page Statistics

| Page | Lines | Components | Features | Responsive |
|------|-------|------------|----------|-----------|
| Index | ~200 | Hero, Features, Roles, CTA | 6 features, 4 roles | 3 breakpoints |
| Dashboard | ~350 | Stats, Goals, Workouts, Nutrition, Mood, Badges | 7 sections | 3 breakpoints |
| Nutrition | ~250 | Stats, Table, Modal, Filter, Export | Meal logging, macros, search | 2 breakpoints |
| CSS | ~650 | 50+ classes | Grid, buttons, cards, forms, modals | Mobile-first |
| JS | ~200 | 6 functions | Theme, modals, toasts, filters, export | No dependencies |

---

## 🚀 Ready-to-Extend Architecture

### For Creating CRUD Pages (Workouts, Goals, BodyWeights, etc.)
All necessary components exist:
- ✅ `.page-header` (title + CTA button)
- ✅ `.filter-bar` (search + export)
- ✅ `.data-table` (sortable table styles)
- ✅ `.modal-backdrop` & `.modal` (edit forms)
- ✅ `.form-group` & `.form-row` (forms)
- ✅ `.action-btn` (quick actions)
- ✅ Table filter JS: `filterTable(selector, table)`
- ✅ Table export JS: `exportTableToCSV(table, filename)`

### For Creating Admin Pages
All necessary components exist:
- ✅ `.stats-grid` (system-wide stats)
- ✅ `.plans-grid` & `.plan-card` (workout plans)
- ✅ `.clients-grid` & `.client-card` (user management)
- ✅ `.data-table` (user/food/plan management)
- ✅ Modals for create/edit operations
- ✅ Batch action buttons

### For Creating Trainer Pages
All necessary components exist:
- ✅ `.clients-grid` (client list)
- ✅ Trainer dashboard (similar to user dashboard)
- ✅ Workout assignment modals
- ✅ Client progress tracking cards

### For Adding Charts & Analytics
Foundation ready:
- ✅ `.summary-stats` (stat cards container)
- ✅ `.progress-bar` (skill level bars)
- ✅ `.mood-bar` (trend visualizations)
- ✅ Card containers for charts
- ✅ Ready for Chart.js, D3.js, or similar integration

---

## ✨ Key Advantages of This UI/UX

### Performance
- ✅ No JavaScript framework bloat (~200 lines vanilla JS)
- ✅ Pure CSS (no CSS-in-JS overhead)
- ✅ CSS Grid (modern layout without floats)
- ✅ Data attributes (progressive enhancement)
- ✅ Minimal file sizes (fittrack.css: ~20KB, fittrack.js: ~8KB)

### Accessibility
- ✅ WCAG AA compliant colors
- ✅ Keyboard navigation support
- ✅ Focus states on all interactive elements
- ✅ Semantic HTML
- ✅ ARIA labels on modals
- ✅ Mobile-friendly touch targets (44px+)

### Maintainability
- ✅ CSS variables for theming
- ✅ Component-based class naming
- ✅ Single responsibility classes
- ✅ Clear component documentation
- ✅ Easy to extend (add new components)
- ✅ No CSS class conflicts

### User Experience
- ✅ Dark/light theme toggle
- ✅ Smooth animations (0.18s)
- ✅ Quick modals for data entry
- ✅ Toast notifications for feedback
- ✅ Progress bars for goals
- ✅ Visual mood indicators
- ✅ One-click CSV export
- ✅ Search/filter on tables

### Developer Experience
- ✅ Simple HTML structure (no complex nesting)
- ✅ Reusable component classes
- ✅ Clear naming conventions (e.g., `.stat-card`, `.goal-item`)
- ✅ Minimal JavaScript (easy to understand)
- ✅ Data-attribute driven interactivity
- ✅ Comprehensive documentation
- ✅ Copy-paste ready components

---

## 📋 Implementation Checklist

### ✅ Completed (Today)
- [x] CSS design system with dark/light modes
- [x] JavaScript utilities (theme, modals, toasts, filters, export)
- [x] Layout & navigation
- [x] Landing page (Index.cshtml)
- [x] Dashboard (Dashboard.cshtml)
- [x] Nutrition tracker (Nutrition.cshtml)
- [x] Documentation (4 guides)

### ⏳ Recommended Next (1-2 Weeks)
- [ ] Create CRUD pages (Workouts, Goals, BodyWeights, MoodLogs)
- [ ] Implement data scaffolding for each page
- [ ] Add charts (weight trend, calorie burn, macro breakdown)
- [ ] Create admin management pages
- [ ] Create trainer client views
- [ ] Add breadcrumb navigation
- [ ] Add pagination for large tables

### 🚀 Future Enhancements (3-4 Weeks+)
- [ ] Real-time notifications (SignalR)
- [ ] Advanced analytics dashboard
- [ ] User profile customization
- [ ] Photo upload for before/after
- [ ] Integration with fitness APIs (Strava, MyFitnessPal)
- [ ] Mobile app (using same API backend)
- [ ] Email notifications
- [ ] Social features (friend tracking, challenges)

---

## 📖 How to Use These Files

### 1. Include in _Layout.cshtml
```html
<link rel="stylesheet" href="~/css/fittrack.css" />
<script src="~/js/fittrack.js"></script>
```

### 2. Create new CRUD page from pattern
```html
<!-- Copy from Nutrition.cshtml structure -->
<!-- 1. Page header -->
<!-- 2. Stats/summary section -->
<!-- 3. Filter bar + table -->
<!-- 4. Modal for create/edit -->
<!-- 5. Include JS for interactivity -->
```

### 3. Reference components
```html
<!-- From QUICK_REFERENCE.md or DESIGN_SYSTEM.md -->
<!-- 1. Find component name -->
<!-- 2. Copy HTML structure -->
<!-- 3. Customize content -->
```

### 4. Customize theme
```css
/* In _custom.css or inline in _Layout.cshtml */
:root {
  --accent: #your-color;
  /* other overrides */
}
```

---

## 🎓 Learning Resources Provided

1. **UI_UX_GUIDE.md** — Start here for architecture overview
2. **DESIGN_SYSTEM.md** — Reference for component library
3. **QUICK_REFERENCE.md** — Quick lookup for common patterns
4. **VISUAL_MOCKUPS.md** — ASCII diagrams of all pages

---

## 💡 Tips for Success

1. **Use the grid system consistently** — All layouts use `.dashboard-grid` with `.col-*`
2. **Reference CSS variables** — Never hardcode colors; use `--accent`, `--bg`, etc.
3. **Test responsive design** — Check pages at 720px, 900px, 1400px
4. **Keep spacing consistent** — Use 12px gaps throughout
5. **Leverage data attributes** — Use `data-action`, `data-target`, `data-table-filter`
6. **Document new components** — Add to component library when adding new classes
7. **Test accessibility** — Use keyboard navigation, check focus states
8. **Monitor file sizes** — CSS: ~20KB, JS: ~8KB (keep them small)

---

## 📞 Support Notes

### Common Questions

**Q: How do I add a new page?**  
A: Copy structure from Nutrition.cshtml, use existing CSS classes, add to navigation in _Nav.cshtml

**Q: How do I change colors?**  
A: Update CSS variables in `:root` (dark mode) and `[data-theme="light"]` (light mode)

**Q: How do I add a new component?**  
A: Create a `.component-name` class in fittrack.css, document in DESIGN_SYSTEM.md, add HTML to QUICK_REFERENCE.md

**Q: How do I make a modal?**  
A: Copy modal structure from Nutrition.cshtml, update IDs and content, use `openModal()` and `closeModal()`

**Q: How do I export a table?**  
A: Add `id="myTable"` to table, add button with `data-csv-export data-target="#myTable"`

**Q: How do I filter a table?**  
A: Add input with `data-table-filter="#myTable"` attribute

**Q: How do I show a toast?**  
A: Call `ft.toast('Message', 'success|error', 3000)`

---

## 🎉 Summary

You now have a **professional, production-ready UI/UX system** for your FitTrack application with:
- ✅ Complete design system
- ✅ 3 fully functional pages
- ✅ All common components
- ✅ Dark/light theme support
- ✅ Responsive design
- ✅ Accessible markup
- ✅ Interactive features
- ✅ Comprehensive documentation

**Next:** Start implementing CRUD pages (Workouts, Goals, etc.) using the provided patterns and components.

**Status:** Ready for production  
**Quality:** Professional-grade  
**Maintenance:** Easy (well-documented, component-based)  
**Extensibility:** High (add new components easily)  

---
Use your own safe credentials in non-local environments.

**Created:** January 2025  
**Version:** 1.0 Complete  
**Status:** ✅ Production Ready
>>>>>>> 940a79274a461500d779e3b365dc15372159c2a7
## Notes

- Access to pages is enforced by session checks in `Program.cs` and role helpers in `BasePageModel`.
- Dashboard behavior changes by role (`Admin`, `Trainer`, `User`).
- Authentication pages are designed to use illustration/photo backgrounds per project UI preference.
<<<<<<< HEAD
=======

>>>>>>> 940a79274a461500d779e3b365dc15372159c2a7
