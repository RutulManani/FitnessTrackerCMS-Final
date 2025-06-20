# 🏋️‍♂️ Fitness Tracker CMS

A robust Content Management System for fitness tracking with complete CRUD functionality for workouts, exercises, and equipment, including proper relationship management.

---

## 📌 Enhanced Features

✅ **Fully Functional CRUD Operations**  
✅ **Proper Many-to-Many Relationship Handling**  
✅ **Comprehensive API Documentation**  
✅ **Fixed Relationship Display Issues**  
✅ **Exercise Count Tracking**  
✅ **Equipment-Exercise Association**  
✅ **Workout-Exercise Management**

---

## 🚀 Key Improvements

### ✅ Fixed Relationship Display Issues
- Exercises now properly show in workout details  
- Equipment now correctly displays associated exercises  
- All counts (`ExerciseCount`, `EquipmentCount`) now work accurately  

### ✅ Enhanced CRUD Operations
- Added proper junction table handling  
- Improved data loading with `Include` / `ThenInclude`  
- Better null checking and error handling  

### ✅ New Relationship Management
- `/Workout/AddExercise` endpoint to associate exercises with workouts  
- Proper counting of related entities in list views  

---

## 🛠 Tech Stack

- **Backend:** ASP.NET Core  
- **Database:** SQL Server with Entity Framework Core  
- **Architecture:** Repository Pattern with DTOs  
- **Relationships:** Proper many-to-many via junction tables  

---

## 🔗 API Endpoints

### Equipment Management

| Endpoint                 | Method | Description                          |
|--------------------------|--------|--------------------------------------|
| `/Equipment`             | GET    | List all equipment                   |
| `/Equipment/Details/{id}`| GET    | Get equipment details with exercises |
| `/Equipment/Create`      | POST   | Create new equipment                 |
| `/Equipment/Edit/{id}`   | PUT    | Update equipment                     |
| `/Equipment/Delete/{id}` | DELETE | Remove equipment                     |

### Exercise Management

| Endpoint                  | Method | Description                           |
|---------------------------|--------|---------------------------------------|
| `/Exercise`               | GET    | List all exercises                    |
| `/Exercise/Details/{id}`  | GET    | Get exercise details with equipment   |
| `/Exercise/Create`        | POST   | Create new exercise                   |
| `/Exercise/Edit/{id}`     | PUT    | Update exercise                       |
| `/Exercise/Delete/{id}`   | DELETE | Remove exercise                       |

### Workout Management

| Endpoint                  | Method | Description                            |
|---------------------------|--------|----------------------------------------|
| `/Workout`                | GET    | List all workouts                      |
| `/Workout/Details/{id}`   | GET    | Get workout details with exercises     |
| `/Workout/Create`         | POST   | Create new workout                     |
| `/Workout/Edit/{id}`      | PUT    | Update workout                         |
| `/Workout/Delete/{id}`    | DELETE | Remove workout                         |
| `/Workout/AddExercise`    | POST   | Add exercise to workout                |

---

## 💾 Database Schema

The system uses three core entities with proper many-to-many relationships:

- **Workouts** – Training session records  
- **Exercises** – Library of exercises  
- **Equipment** – Fitness tools catalog  

**Relationships:**

- **Workout ↔ Exercise** (M:M via `WorkoutExercise` junction table)  
- **Exercise ↔ Equipment** (M:M via `ExerciseEquipment` junction table)  



