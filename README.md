# 📝 Journal Entry Task - Project and Setup Guide

This project focuses on creating a comprehensive interface for entering accounting **Journal Entries**, structured as two separate applications: a **Front-End** using Angular 20 and a **Back-End** API using ASP.NET 9.

## ✨ Core Features and Requirements Implementation

All core functional requirements have been implemented and validated, including:

* **Entry Balance Validation:** Checking that the total **Debit (المدين)** equals the total **Credit (الدائن)** before saving the entry.
* **Auto-Complete:** Automatic listing and search for Account Names and Account Numbers sourced from the `AccountsChart` table.
* **Local Storage:** Temporary saving of user input data in the browser's `Local Storage` to prevent data loss upon refresh.
* **Debit/Credit Logic:** Automatic zeroing of the opposite field when a value is entered in either the Debit or Credit column.
* **Data Integrity:** Implementing a **One-to-Many** relationship between the Journal Header and its Details in the database.

---

## 🛠️ Technologies and Environment

| Project            | Technology | Version |
|                    |            |         |
| **Front-End (UI)** | Angular    | 20      |
| **Back-End (API)** | ASP.NET    | 9       |
| **Database (DB)**  | SQL Server |         |

---

## 🚀 Setup and Running Instructions

Please follow the steps below precisely to ensure both the API and the UI are configured and running correctly.

### 1. Database Setup (SQL Server)

Two SQL scripts are provided. The execution order is **critical** due to table dependencies.

1.  **Script (1a) - Create Database Only (My Script):**
    * Execute the first part of your own script to **create the database** named **`fCarePlus`**.
    * **STOP HERE.** Do not execute the remaining table creation commands yet.
2.  **Script (2) - Create Accounts Chart and Insert Data (Provided Script):**
    * Execute the **`AccountsChart`** script (the one provided in the task requirements).
    * This script creates the **`AccountsChart`** table and populates it with data required for the auto-complete feature.
3.  **Script (1b) - Create Remaining Tables (My Script):**
    * Return to your initial script and execute the remaining commands.
    * This creates the primary tables: **`JournalHeader`** and **`JournalDetails`**.
4.  **Update Connection String:**
    * In the Back-End project's configuration file (e.g., `appsettings.json`), update the **Connection String** to match your local SQL Server settings.

---

### 2. Running the Back-End (API - ASP.NET 9)

1.  Open the Back-End project folder in **Visual Studio**.
2.  Ensure the Connection String is correctly configured (Step 1.4).
3.  **Run** the project (e.g., using F5 or Ctrl+F5) to start the API server.

---

### 3. Running the Front-End (UI - Angular 20)

1.  Navigate to the Front-End project folder in your terminal (CLI).
2.  **Install Dependencies:** Run the following command to install all necessary  Node.js packages:
    ```bash
    npm install
    ```
3.  **Verify API URL:** Check the environment files (e.g., `environment.ts`) and confirm the base API URL points to your running Back-End .
4.  **Run Application:** Start the Angular development server with the command:
    ```bash
    ng serve
    ```

#### **Accessing the Application:**

Once both projects are running, you can access the user interface in your web browser (typically at `http://localhost:4200`).