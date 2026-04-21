# FitTrack UI/UX — Visual Layout Guide

## 📱 Landing Page (Index.cshtml) — Desktop View

```
┌────────────────────────────────────────────────────────────────┐
│  FitTrack    [Home] [Dashboard] [Nutrition] [Goals]   🌙 [Login]│
└────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│                         HERO SECTION                            │
│ ┌─────────────────────────────────┬──────────────────────────┐ │
│ │  ✨ Build Your Best Self       │                          │ │
│ │                                │                          │ │
│ │  Your Complete Fitness Journey │        💪              │ │
│ │  Starts Here                   │      (floating)          │ │
│ │                                │                          │ │
│ │  Track workouts, log nutrition,│                          │ │
│ │  monitor mood, and achieve     │                          │ │
│ │  goals—all in one beautiful... │                          │ │
│ │                                │                          │ │
│ │  [Get Started Free] [Sign In]  │                          │ │
│ │                                │                          │ │
│ │  5000+ Users | 50K+ Workouts  │                          │ │
│ │  100K+ Meals                   │                          │ │
│ └─────────────────────────────────┴──────────────────────────┘ │
└──────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│                      FEATURES SECTION                           │
│                   Everything You Need                           │
│                                                                  │
│  ┌───────────────┬───────────────┬───────────────┐             │
│  │ ⚖️ Weight     │ 🍎 Nutrition  │ 🏋️ Workouts  │             │
│  │ Tracking      │ Logging       │ Tracking      │             │
│  │               │               │               │             │
│  │ Log daily &   │ Track meals   │ Follow plans  │             │
│  │ visualize     │ with local    │ & log every   │             │
│  │ progress      │ foods         │ session       │             │
│  └───────────────┴───────────────┴───────────────┘             │
│                                                                  │
│  ┌───────────────┬───────────────┬───────────────┐             │
│  │ 🎯 Goals      │ 😊 Mood       │ 🏆 Gamification             │
│  │ Management    │ Tracking      │                │             │
│  │               │               │                │             │
│  │ Set goals &   │ Monitor mood  │ Earn badges,  │             │
│  │ track with    │ & energy to   │ maintain      │             │
│  │ visual bars   │ guide you     │ streaks       │             │
│  └───────────────┴───────────────┴───────────────┘             │
└──────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│                      ROLES SECTION                              │
│                    Built for Everyone                           │
│                                                                  │
│  ┌──────────────┬──────────────┬──────────────┬──────────────┐ │
│  │ 👤 Fitness   │ 👨‍🏫 Trainers   │ ⚙️ Admins     │ 🌍 Everyone   │ │
│  │ Enthusiasts  │              │              │              │ │
│  │              │              │              │              │ │
│  │ Track every  │ Manage       │ Manage users,│ Simple tools │ │
│  │ workout,     │ clients,     │ foods,       │ to start &   │ │
│  │ meal, &      │ assign       │ plans, with  │ maintain a   │ │
│  │ milestone    │ workouts, &  │ full control │ healthier    │ │
│  │              │ monitor      │              │ lifestyle    │ │
│  └──────────────┴──────────────┴──────────────┴──────────────┘ │
└──────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│                      CTA SECTION                                │
│                                                                  │
│           Ready to Transform Your Fitness?                     │
│   Join thousands of users tracking their health & achieving    │
│                                                                  │
│                [Start Your Free Account]                       │
│                                                                  │
└──────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│  © 2025 FitTrack | Privacy | Terms | Support                   │
└──────────────────────────────────────────────────────────────────┘
```

---

## 📊 Dashboard (Dashboard.cshtml) — Desktop View

```
┌────────────────────────────────────────────────────────────────┐
│  FitTrack    [Home] [Dashboard] [Nutrition] [Goals]   🌙 [User]│
└────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│ Welcome back, Alice! 👋                    [+ Log Workout]     │
│ Here's your fitness overview for today                          │
└──────────────────────────────────────────────────────────────────┘

┌─────────────┬──────────────┬──────────────┬──────────────┐
│ ⚖️ Weight   │ 🔥 Calories  │ 💪 Workouts  │ 😊 Mood      │
│ Current     │ Today        │ This Week    │ Today        │
│ 72.5 kg     │ 1,850 kcal   │ 4            │ 4/5          │
│ ↓ 2kg start │ Goal: 1,500  │ Goal: 5      │ Feeling      │
│             │ +350 over    │ 1 more       │ great!       │
└─────────────┴──────────────┴──────────────┴──────────────┘

┌─────────────────────────────────────────┬──────────────────────┐
│           🎯 YOUR GOALS (2/3)           │   🏆 BADGES (1/3)   │
│ ┌─────────────────────────────────────┐│ ┌──────────────────┐ │
│ │ Lose 5kg                            ││ │ 🏋️ 🔥 🥗 ⚖️ 🎉  │ │
│ │ ████░░░░ 45% | Aug 1                ││ │ 📈 🌟          │ │
│ │ 70.5kg → 67.5kg                     ││ │ 4 of 12 earned │ │
│ │                                     ││ │                │ │
│ │ Run 5km Daily                       ││ └──────────────────┘ │
│ │ ██████░░ 60% | Jul 1                │                      │
│ │ 3km → 5km                           │                      │
│ └─────────────────────────────────────┘│                      │
└─────────────────────────────────────────┴──────────────────────┘

┌─────────────────────────────────────────┬──────────────────────┐
│     💪 RECENT WORKOUTS (1/2)            │  🍎 NUTRITION (1/2)  │
│ ┌─────────────────────────────────────┐│ ┌──────────────────┐ │
│ │ Morning Cardio                      ││ │ Breakfast        │ │
│ │ 35 min • 280 kcal ✓ (May 20)       ││ │ 150 kcal ✓       │ │
│ │                                     ││ │                  │ │
│ │ Strength Builder                    ││ │ Lunch            │ │
│ │ 45 min • 320 kcal ✓ (May 21)       ││ │ 694 kcal ✓       │ │
│ │                                     ││ │                  │ │
│ │ HIIT Blast                          ││ │ Dinner           │ │
│ │ 30 min • 250 kcal ✓ (May 22)       ││ │ 247 kcal ✓       │ │
│ │                                     ││ │                  │ │
│ │ [View all →]                        ││ │ [Log meal →]     │ │
│ └─────────────────────────────────────┘│ └──────────────────┘ │
└─────────────────────────────────────────┴──────────────────────┘

┌─────────────────────────────────────────┬──────────────────────┐
│      😊 MOOD TODAY (1/2)                │  🤖 INSIGHTS (1/2)   │
│ ┌─────────────────────────────────────┐│ ┌──────────────────┐ │
│ │ Mood Score                          ││ │ 🎉 Great progress!  │
│ │ ████████░░░ 4/5 (Feeling great!)   ││ │ You're on track   │
│ │                                     ││ │ to lose 5kg by    │
│ │ Energy Level                        ││ │ August. Keep up 1 │
│ │ ██████████ 5/5 (Very energetic!)   ││ │ more workout/wk   │
│ │                                     ││ │                  │ │
│ │                                     ││ │ 💡 Nutrition Tip   │
│ │                                     ││ │ Add more protein  │
│ │                                     ││ │ to breakfast      │
│ │                                     ││ │                  │ │
│ └─────────────────────────────────────┘│ └──────────────────┘ │
└─────────────────────────────────────────┴──────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│ Quick Actions:                                                   │
│ [+ Workout] [+ Meal] [+ Goal] [+ Mood] [+ Weight]              │
└──────────────────────────────────────────────────────────────────┘
```

---

## 🍎 Nutrition Tracker (Nutrition.cshtml) — Desktop View

```
┌────────────────────────────────────────────────────────────────┐
│  FitTrack    [Home] [Dashboard] [Nutrition] [Goals]   🌙 [User]│
└────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│ 🍎 Nutrition Tracker                      [+ Add Meal]         │
└──────────────────────────────────────────────────────────────────┘

┌─────────────┬─────────────┬──────────────┬────────────────┐
│ 🔥  1,850   │ 🥚 58g      │ 🌾 258g      │ 🧈 42g         │
│ kcal        │ Protein     │ Carbs        │ Fat            │
│ Goal: 1,500 │ Goal: 120g  │ Goal: 180g   │ Goal: 50g      │
│ +350 kcal   │ 48% ✓       │ 143% ⚠️      │ 84% ✓          │
└─────────────┴─────────────┴──────────────┴────────────────┘

[Search meals...........................] [📊 Export]

┌────────────────────────────────────────┬──────────────────────┐
│         🌅 BREAKFAST (1/2)             │   🌞 LUNCH (1/2)     │
│ ┌──────────────────────────────────┐ │ ┌──────────────────┐ │
│ │ Banana (Igitoki)                 │ │ │ Beans (Ibishyimbo) │
│ │ 150g • 133.5 kcal • 1.65g protein │ │ │ 200g • 694 kcal    │
│ │ [Delete]                         │ │ │ [Delete]           │
│ │                                  │ │ │                    │
│ │ Milk (Amata)                     │ │ │ Subtotal:          │
│ │ 100ml • 61 kcal • 3.2g protein  │ │ │ 694 kcal           │
│ │ [Delete]                         │ │ │                    │
│ │                                  │ │ │                    │
│ │ Subtotal: 194.5 kcal             │ │ │                    │
│ └──────────────────────────────────┘ │ └──────────────────┘ │
└────────────────────────────────────────┴──────────────────────┘

┌────────────────────────────────────────┬──────────────────────┐
│         🌙 DINNER (1/2)                │ 🇷🇼 LOCAL FOODS      │
│ ┌──────────────────────────────────┐ │ ┌──────────────────┐ │
│ │ Chicken Breast                   │ │ │ Ugali            │
│ │ 150g • 247.5 kcal • 46.5g protein│ │ │ 360 kcal/100g    │
│ │ [Delete]                         │ │ │                  │
│ │                                  │ │ │ Isombe           │
│ │ Subtotal: 247.5 kcal             │ │ │ 45 kcal/100g     │
│ │                                  │ │ │                  │
│ │                                  │ │ │ Potatoes         │
│ │                                  │ │ │ 77 kcal/100g     │
│ │                                  │ │ │                  │
│ │                                  │ │ │ Beans            │
│ │                                  │ │ │ 347 kcal/100g    │
│ └──────────────────────────────────┘ │ └──────────────────┘ │
└────────────────────────────────────────┴──────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│ [← Dashboard] [⚖️ Weight Log] [💪 Workouts]                     │
└──────────────────────────────────────────────────────────────────┘
```

### Add Meal Modal (Overlay)

```
                    ╔═══════════════════════════════════╗
                    ║ Add Meal                      [×] ║
                    ╠═══════════════════════════════════╣
                    ║                                   ║
                    ║ Meal Type:                        ║
                    ║ [🌅 Breakfast ▼]                 ║
                    ║                                   ║
                    ║ Food Item:                        ║
                    ║ [Search or enter food name...]   ║
                    ║                                   ║
                    ║ Amount:          Unit:            ║
                    ║ [100_____]       [g ▼]           ║
                    ║                                   ║
                    ║ Time:                             ║
                    ║ [08:00__]                         ║
                    ║                                   ║
                    ║ Notes (optional):                 ║
                    ║ ┌───────────────────────────────┐ ║
                    ║ │                               │ ║
                    ║ │                               │ ║
                    ║ └───────────────────────────────┘ ║
                    ║                                   ║
                    ║         [Cancel]  [Save Meal]    ║
                    ║                                   ║
                    ╚═══════════════════════════════════╝
```

---

## 📱 Mobile View (720px) — Dashboard

```
┌──────────────────────────┐
│ FitTrack [☰]  🌙 [User]  │
└──────────────────────────┘

┌──────────────────────────┐
│ Welcome back, Alice! 👋  │
│ Your fitness overview    │
│        [+ Workout]       │
└──────────────────────────┘

┌──────────────────────────┐
│ ⚖️ Weight                │
│ 72.5 kg                  │
│ ↓ 2kg from start         │
└──────────────────────────┘

┌──────────────────────────┐
│ 🔥 Calories              │
│ 1,850 kcal               │
│ Goal: 1,500 | +350       │
└──────────────────────────┘

┌──────────────────────────┐
│ 💪 Workouts              │
│ 4 this week              │
│ 1 more to goal           │
└──────────────────────────┘

┌──────────────────────────┐
│ 😊 Mood                  │
│ 4/5 - Feeling great!     │
└──────────────────────────┘

┌──────────────────────────┐
│ 🎯 Your Goals            │
│                          │
│ Lose 5kg                 │
│ ████░░░░ 45%             │
│ Aug 1                    │
│                          │
│ Run 5km Daily            │
│ ██████░░ 60%             │
│ Jul 1                    │
└──────────────────────────┘

┌──────────────────────────┐
│ 🏆 Badges (4/12)         │
│ 🏋️ 🔥 🥗 ⚖️             │
│ 🎉 📈                    │
└──────────────────────────┘

┌──────────────────────────┐
│ 💪 Recent Workouts       │
│                          │
│ Morning Cardio           │
│ 35 min • 280 kcal ✓      │
│                          │
│ Strength Builder         │
│ 45 min • 320 kcal ✓      │
│                          │
│ HIIT Blast               │
│ 30 min • 250 kcal ✓      │
│                          │
│ [View all →]             │
└──────────────────────────┘

┌──────────────────────────┐
│ 🍎 Today's Nutrition     │
│                          │
│ Breakfast: 150 kcal ✓    │
│ Lunch: 694 kcal ✓        │
│ Dinner: 247 kcal ✓       │
│                          │
│ [Log meal →]             │
└──────────────────────────┘

┌──────────────────────────┐
│ 😊 Mood Today            │
│                          │
│ Mood: ████████░░░ 4/5   │
│ Energy: ██████████ 5/5  │
└──────────────────────────┘

┌──────────────────────────┐
│ 🤖 Smart Insights        │
│                          │
│ 🎉 Great progress!       │
│ You're on track to lose  │
│ 5kg by August. Keep up   │
│ 1 more workout/wk.       │
│                          │
│ 💡 Nutrition Tip         │
│ Add more protein to      │
│ breakfast.               │
└──────────────────────────┘

┌──────────────────────────┐
│ Quick Actions:           │
│ [+ Workout]              │
│ [+ Meal]                 │
│ [+ Goal]                 │
│ [+ Mood]                 │
│ [+ Weight]               │
└──────────────────────────┘
```

---

## 🎨 Color & Typography Reference

### Dark Mode (Default)
```
Background: Linear gradient from #0f1724 (darker on edges)
Cards: #0b1220
Text: #e6eef6 (light)
Muted: #9aa4b2 (gray-blue)
Accent: #3b82f6 (blue CTA)
Success: #10b981 (green badges)
Danger: #ef4444 (red alerts)
```

### Light Mode
```
Background: Linear gradient from #f6f8fb
Cards: #ffffff (white)
Text: #0b1320 (dark)
Muted: #56626d (dark gray)
Accent: #2563eb (darker blue)
Success: #059669 (darker green)
Danger: #dc2626 (darker red)
```

### Font Sizes
```
h1: 2.8rem (280% of 1rem)
h2: 1.8rem (180%)
h3: 1.2rem (120%)
Body: 1rem (100%)
Small: 0.9rem (90%)
Tiny: 0.85rem (85%)
```

---

## ✨ Animation & Interaction

### Hover States
- Buttons: Opacity shift + subtle shadow
- Cards: Slight lift effect (shadow depth)
- Links: Color underline

### Focus States
- All interactive: 3px outline in accent color
- Offset: 2px from edge
- Color: Accent with 25% opacity

### Transitions
- Standard: 0.18s ease
- Modal backdrop: Fade 0.18s ease
- Dropdown: Slide 0.15s ease

---

## 📐 Spacing Grid

```
Base unit: 4px (not used directly)
Common: 8, 10, 12, 14, 16, 18, 20, 24, 28px

Component spacing:
- Button padding: 8px 12px (small), 12px 20px (large)
- Card padding: 12-18px
- Section gap: 12px (standard)
- Margin bottom: 18-28px (sections)
```

---

## 🚀 Summary

✅ **3 complete pages** with all features showcased
✅ **Professional design system** with dark/light modes
✅ **Responsive layouts** for all screen sizes
✅ **Interactive components** (modals, filters, exports)
✅ **Accessible** (ARIA labels, focus states, keyboard nav)
✅ **Fast** (CSS Grid, minimal JS, smooth transitions)
✅ **Ready to extend** (all components documented)

