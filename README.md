## How to install

This project was developed around SSMS, here's the procedure to get it to work. 

1. Download and extract the project folder. 
2. Place the DLISH folder containing the application in a directory on your computer. 
3. Open SSMS and open a new query on the server. 
4. Paste the contents of DLISH.sql into the new query, select the entire query and run it. 
5. Close SSMS and open the Takeaway project solution. 
6. In View > "SQL Server Object Explorer", open the tree to the table display. 
7. Right-click > "properties" on the GroffierKhloufi database. 
8. Copy the connection string to the clipboard. 
9. In appsettings.json, modify the string connection with the clipboard connection.

Start and enjoy the program.

## Description
The **DLISH Takeaway Project** is a web application that allows users to browse restaurants and place orders online.

- **For Customers:**  
  - Browse available restaurants.  
  - View menus and select dishes.  
  - Place orders directly through the website.  

- **For Restaurant Owners:**  
  - Manage menus and update available dishes.  
  - Track and manage incoming orders efficiently.  

## Pre-created Accounts

The project comes with several pre-created accounts to test both customer and restaurant functionalities.

### Restaurateurs
| Email | Password |
|-------|---------|
| jean.dupont@example.com | `hashed_password_123` |
| marie.leclerc@example.com | `hashed_password_456` |
| pierre.martin@example.com | `hashed_password_789` |
| sophie.dubois@example.com | `hashed_password_012` |
| luc.renaud@example.com | `hashed_password_345` |

Here is a quick look at how the restaurateur can use our website (click to enlarge).

<img src="https://github.com/user-attachments/assets/8b4f8790-9c93-4e35-83b7-37eac70d4195" width="60%">

### Clients
| Email | Password |
|-------|---------|
| alice.bonnet@example.com | `hashed_password_678` |
| bob.leroy@example.com | `hashed_password_901` |
| claire.durand@example.com | `hashed_password_234` |
| david.fournier@example.com | `hashed_password_567` |
| emma.roux@example.com | `hashed_password_890` |
| felix.girard@example.com | `hashed_password_1234` |
| gabrielle.noel@example.com | `hashed_password_5678` |
| hugo.lambert@example.com | `hashed_password_9012` |
| isabelle.morin@example.com | `hashed_password_3456` |
| julien.petit@example.com | `hashed_password_7890` |

Here is a quick look at how the client can use our website (click to enlarge).

<img src="https://github.com/user-attachments/assets/8f309287-d7f3-4e8f-84aa-0efd07603a96" width="60%">

