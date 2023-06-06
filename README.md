# EMR DATA TOOL: Running MYSQL Scripts with EMR Tools

## Description:
This project aims to provide a streamlined solution for running MYSQL scripts using EMR (Electrinic medical records) Tools in a C# environment. It allows users to easily execute SQL scripts on a MYSQL database using the power and scalability of EMR. The project also includes features for sending SMS reminders for appointment reminders and collecting feedback from clients.

## Features:
1. **Running MYSQL Scripts**: Execute SQL scripts on a MYSQL database using EMR Tools. The project provides a simple interface for specifying the script file, database credentials, and EMR configuration.

2. **EMR Integration**: Utilizes EMR  Tools to distribute and parallelize the execution of SQL scripts, ensuring optimal performance and scalability for large datasets.

3. **SMS Reminder Service**: Clients can receive SMS reminders for their appointments. The system includes a mechanism to schedule reminders and send them via SMS using an integrated SMS gateway.

4. **Feedback Collection**: The project includes a feedback mechanism that allows clients to provide feedback on their experience. This feedback can be valuable for improving the service and enhancing customer satisfaction.

## Prerequisites:
- A MYSQL database with the necessary tables and schema.
- Access to an EMR cluster with MYSQL and necessary tools installed.
- Proper configuration and credentials for connecting to the MYSQL database (openmrs)
- An SMS gateway account for sending SMS reminders/ Alternatively you can use modem

## Installation and Setup:
1. Clone the repository from GitHub.
2. Ensure that you have the necessary .NET and C# development environment set up.
3. Restore the project dependencies mentioned in the project's .csproj file.
4. Configure the MYSQL database connection details in the config.ini file.
5. Configure the EMR cluster details in the emr_config.ini file.
6.  Set up the SMS gateway account and configure the necessary credentials in the sms_config.ini file.
7. Build and run the project.

## Usage:
1. Running MYSQL Scripts:
   - Place your SQL script file in the project directory.
   - Modify the `config.ini` file with the appropriate database credentials.
   - Build and run the project.
   - Select the "Run MYSQL Script" option.
   - Provide the script file name when prompted.
   - The script will be executed on the MYSQL database using EMR Tools.

2. SMS Reminder Service:
   - Ensure that the SMS gateway account and credentials are properly configured in the `sms_config.ini` file.
   - Build and run the project.
   - Select the "Schedule SMS Reminder" option.
   - Provide the necessary details like recipient's phone number, appointment time, and reminder message.
   - The system will schedule the reminder and send an SMS at the specified time.

3. Feedback Collection:
   - After the completion of an appointment or service, provide clients with a way to send feedback.
   - Collect the feedback using an appropriate user interface or form.
   - Store the feedback securely for analysis and improvements.

## Contributors:
- [Victor Owino](https://github.com/VictorOwinoKe)

## License:
This project is licensed under the [MIT License](LICENSE).

Please refer to the project's individual files and documentation for more detailed instructions and guidelines. If you encounter any issues or have suggestions, feel free to open an issue on the GitHub repository.
