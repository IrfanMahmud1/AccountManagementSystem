# 💼 Mini Account Management System

A lightweight accounting system built with **ASP.NET Core Razor Pages** and **MS SQL Server**, using **Stored Procedures only** (no LINQ). The system includes user role management, chart of accounts with hierarchical support, and voucher entry for journal, payment, and receipt transactions.

---

## 🛠️ Technologies Used

- ✅ ASP.NET Core (Razor Pages)
- ✅ MS SQL Server
- ✅ Stored Procedures (No LINQ)
- ✅ ASP.NET Core Identity (Custom Roles & Permissions)
- ✅ Bootstrap 5 (UI Components)
- ✅ Entity Framework Core (only for Identity user management)
- ✅ Serilog (optional for logging)

---

## 🔐 Authentication & Authorization

- **User Roles:**
  - `Admin`: Full control over users, roles, and vouchers
  - `Accountant`: Can manage chart of accounts and vouchers
  - `Viewer`: Read-only access

- **Role-Based Access Control:**
  - Assign module-level access via stored procedures
  - Authorization enforced in Razor Pages using `[Authorize(Roles = "...")]`

---

## 🧑‍💼 User & Role Management

- Assign role permissions via `sp_ManageRoleModuleAccess`

🖼️ Screenshot:
![User & Role Management](/docs/screenshots/AccessByAdmin.bmp)
![User & Role Management](/docs/screenshots/AccessByViewer.bmp)
![User & Role Management](/docs/screenshots/AccessByCOA.bmp)
![User & Role Management](/docs/screenshots/RoleModuleAccessList.bmp)
![User & Role Management](/docs/screenshots/CreateRMA.bmp)
![User & Role Management](/docs/screenshots/EditRMA.bmp)
![User & Role Management](/docs/screenshots/DeleteRMA.bmp)

---

## 🧾 Chart of Accounts

- Create, update, delete accounts such as:
  - `Cash`, `Bank`, `Accounts Receivable`
- Support for **parent-child hierarchy** (Tree view)
- All operations handled by `sp_ManageChartOfAccounts`

🖼️ Screenshot:
![Chart of Accounts Tree](/docs/screenshots/ChartOfAccountList.bmp)
![Chart of Accounts Tree](/docs/screenshots/CreateCOA.bmp)
![Chart of Accounts Tree](/docs/screenshots/EditCOA.bmp)

---

## 🧮 Voucher Entry Module

Supports the following voucher types:
- 📘 Journal Voucher
- 💵 Payment Voucher
- 💰 Receipt Voucher

### Form Fields
- Voucher Date
- Reference No.
- Voucher Type (dropdown)
- **Multi-line** Debit & Credit Entry Table:
  - Account dropdown
  - Amount
  - Narration (optional)

### Stored Procedure
- `sp_SaveVoucher` for inserting vouchers and associated debit/credit lines

🖼️ Screenshot:
![Voucher Entry Form](/docs/screenshots/VoucherLIst.bmp)
![Voucher Entry Form](/docs/screenshots/CreateVoucher.bmp)

---

## 🔄 Pagination Support

To improve performance and user experience when working with large datasets (e.g., vouchers, accounts), **server-side pagination** is implemented using stored procedures.

- Applied in:
  - Chart of Accounts listing
  - Voucher list pages
  - User and role management
- Efficient page loading with stored procedures such as `sp_GetPaginatedVouchers`

🖼️ Screenshot:
![Pagination Example](/docs/screenshots/Pagination.bmp)

---

## 📂 Database Design (Simplified)

- **Users / Roles**: ASP.NET Identity Tables
- **ChartOfAccounts**
  - `Id`, `Name`, `ParentId`, `Date`, `AccountType`, `CreatedAt`, `IsActive`
- **Vouchers**
  - `Id`, `Date`, `ReferenceNo`, `Type`
- **VoucherEntries**
  - `Id`, `VoucherId`, `AccountName`, `Debit`, `Credit`

---

## 🚀 Getting Started

### Prerequisites

- .NET SDK 7.0 or later
- MS SQL Server (LocalDB or full version)
- Visual Studio 2022+

### Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/IrfanMahmud1/AccountManagementSystem.git
   cd MiniAccountSystem

2. Configure appsettings.json:
   ```bash
   "ConnectionStrings"{
   "DefaultConnection": "Server=.\\SQLEXPRESS;Database=AppQtech;Trusted_Connection=True;Trust Server Certificate=true;MultipleActiveResultSets=true"
   }
3. Update database:
   ```bash
   dotnet ef database update --project App.Qtech.Infrastructure --startup-project App.Qtech.Web

4. Run the application:
   ```
   dotnet run