# Community Sports Facilities Booking System (SFBS)

An ASP.NET Core MVC web application with a Microsoft SQL Server backend that allows residents to search, book and review community sports facilities managed by a local sports council.

Developed as coursework for the **Data and Web Development** module — ESOFT Metro Campus / London Metropolitan University.

- **Student ID:** E293034
- **Module Leader:** Mr. Nipuna
- **Assignment:** 001 (100% of module mark)

---

## Overview

Community sports facilities such as tennis courts, football pitches and basketball courts are traditionally booked by phone using paper diaries. This leads to double bookings and gives the council no reliable view of facility usage. This system replaces that manual process with a web application that supports two user types:

| User type | Capabilities |
|---|---|
| **Guest** (unregistered) | Restricted facility search, view facility reviews, register as a member, send an inquiry to the council |
| **Member** (registered) | Sign in, full facility search by type/location/date/time, book a facility, submit a review after use |

---

## Features

- Home page acting as a single entry point for both guests and members
- Member registration with a unique-email constraint
- Member sign in / sign out with an adaptive navigation bar
- Facility search with results rendered as cards
- Restricted guest view — "Sign in to Book" replaces the live Book button
- Booking form capturing date, start time and end time
- Review submission with a 1–5 star rating and free-text comments
- Guest inquiry form written directly to the `Inquiry` table
- Normalised five-table relational schema with referential integrity

---

## Technology Stack

| Layer | Technology |
|---|---|
| Presentation | Razor Views (`.cshtml`), HTML5, CSS3, Bootstrap |
| Application | ASP.NET Core MVC (C#) |
| Data access | ADO.NET via a shared `DbHelper` / `DatabaseContext` class |
| Database | Microsoft SQL Server (SSMS) |
| Modelling | Oracle SQL Developer Data Modeler, SSMS Database Diagrams |
| IDE | Visual Studio 2022 |

---

## Project Structure

```
SportsBookingMV/
├── Controllers/
│   ├── HomeController.cs
│   ├── AccountController.cs
│   ├── FacilityController.cs
│   ├── BookingController.cs
│   └── ReviewController.cs
├── Models/
│   ├── MemberModel.cs
│   ├── FacilityModel.cs
│   ├── BookingModel.cs
│   ├── ReviewModel.cs
│   └── InquiryModel.cs
├── Views/
│   ├── Home/
│   ├── Account/
│   ├── Facility/
│   └── Shared/
│       └── _Layout.cshtml
├── Helpers/
│   └── DbHelper.cs
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── images/
├── Database/
│   ├── 01_CreateDatabase.sql
│   ├── 02_InsertSampleData.sql
│   └── 03_SelectQueries.sql
├── appsettings.json
└── Program.cs
```

---

## Database Design

The database `SportsBookingDB` contains five tables.

| Table | Primary key | Foreign keys |
|---|---|---|
| `Facility` | `FacilityID` | — |
| `Members` | `MemberID` | — |
| `Booking` | `BookingID` | `MemberID` → `Members`, `FacilityID` → `Facility` |
| `Review` | `ReviewID` | `MemberID` → `Members`, `FacilityID` → `Facility` |
| `Inquiry` | `InquiryID` | — (guest details stored on the row) |

### Relationships

- One member can make many bookings — `Member 1 : M Booking`
- One facility can have many bookings — `Facility 1 : M Booking`
- One member can write many reviews — `Member 1 : M Review`
- One facility can receive many reviews — `Facility 1 : M Review`
- `Inquiry` is standalone, so an unregistered guest can contact the council

### Constraints

- `UNIQUE` on `Members.Email` — prevents duplicate registrations
- `CHECK (Rating BETWEEN 1 AND 5)` on `Review.Rating`
- `DEFAULT 'Confirmed'` on `Booking.Status`
- `DEFAULT GETDATE()` on `Review.ReviewDate` and `Inquiry.InquiryDate`
- `DEFAULT 1` on `Facility.IsAvailable`

---

## Setup Instructions

### Prerequisites

- Visual Studio 2022 with the ASP.NET and web development workload
- .NET SDK (version matching the project's target framework)
- Microsoft SQL Server (Express or Developer edition)
- SQL Server Management Studio (SSMS)

### 1. Clone the repository

```bash
git clone <repository-url>
cd SportsBookingMV
```

### 2. Create the database

Open SSMS, connect to your local SQL Server instance and run the scripts in the `Database` folder in order:

```
01_CreateDatabase.sql      -- creates SportsBookingDB and the five tables
02_InsertSampleData.sql    -- loads sample facilities, members, bookings and reviews
```

### 3. Configure the connection string

Update `appsettings.json` with your own server name:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=SportsBookingDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

If you use SQL Server authentication instead of Windows authentication, replace `Trusted_Connection=True` with `User Id=your_user;Password=your_password`.

### 4. Run the application

```bash
dotnet restore
dotnet run
```

Or press **F5** in Visual Studio. The application opens at `https://localhost:xxxx`.

---

## Testing

Ten functional test cases were executed manually against the running application, with the database state verified after each action. All ten passed.

| ID | Test case | Status |
|---|---|---|
| TC01 | Member registration with valid details | Pass |
| TC02 | Registration with a duplicate email | Pass |
| TC03 | Member sign in with correct credentials | Pass |
| TC04 | Member sign in with incorrect password | Pass |
| TC05 | Guest searches facilities | Pass |
| TC06 | Guest attempts to book without signing in | Pass |
| TC07 | Member books a facility | Pass |
| TC08 | Member submits a review | Pass |
| TC09 | Guest sends an inquiry | Pass |
| TC10 | Guest views facility reviews | Pass |

---

## Scope and Limitations

Implemented: all guest and member functions described in the case study.

Out of scope for this prototype, and proposed as future enhancements:

- Administrative dashboard for the sports council to approve bookings and manage facilities
- Double-booking prevention through overlap validation on the booking slot
- Password hashing and stronger session/authentication security
- Email confirmation of bookings and inquiries
- Payment integration for the hourly rate

---

## References

- Connolly, T. and Begg, C. (2015) *Database Systems: A Practical Approach to Design, Implementation, and Management*. 6th edn. Harlow: Pearson Education.
- Elmasri, R. and Navathe, S.B. (2016) *Fundamentals of Database Systems*. 7th edn. Harlow: Pearson Education.
- Freeman, A. (2016) *Pro ASP.NET Core MVC*. 6th edn. New York: Apress.
- Microsoft (2026) *Overview of ASP.NET Core MVC*. Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/overview
- Microsoft (2026) *Transact-SQL Reference (Database Engine)*. Available at: https://learn.microsoft.com/en-us/sql/t-sql/language-reference

---

## Academic Declaration

This repository contains coursework submitted for assessment. It is provided for academic review only and should not be reused or submitted elsewhere.
