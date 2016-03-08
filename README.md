# carriers
Training carriers rate app for Axado company

This app was created to show a little bit of my C# programming skills for the purpose to be hired by Axado company.

This app was developed to rate carriers. To run this app, there is no database configuration needed. Once you download the app, the connection string is already configured and the data will be stored in a secure database.

Business rules of the app:
--------------------------
To use this app you must be logged, and there is already an Administrator user registered on the app. The login is 'admin' and the password is 'admin'. This user has the power of register another users, because his role is 'A-Admin'. All the users configured with the role 'U-User' cannot register another user and the User button is invisible for these users.

After register some Carriers, the users can rate these Carriers. To do so, they have to go under Rates link and choose a Carrier and give it a rate. The user can rate only once each Carrier.
