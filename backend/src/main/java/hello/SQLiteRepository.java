package hello;

import java.sql.*;
import java.util.ArrayList;

public class SQLiteRepository {
    private static Connection connection;
    private static boolean hasData;
    private int userId;

    public ResultSet displayUsers() throws SQLException, ClassNotFoundException {
        checkConnection();

        return connection.createStatement().executeQuery("SELECT * FROM Users ");
    }

    public ResultSet displayDevices(String username, String password) throws SQLException, ClassNotFoundException {
        checkConnection();
        ResultSet devices = connection.createStatement().executeQuery(String.format(
                "SELECT deviceId, name, targetTemperature, realTemperature  FROM " +
                        "(SELECT * FROM Users JOIN UserDevice ON Users.id = userId JOIN Devices ON deviceId = Devices.id)" +
                        "WHERE username='%s' AND password='%s'",
                username, password));

        return devices;
    }


    private void checkConnection() throws SQLException {
        if (connection != null) return;
        try {
            Class.forName("org.sqlite.JDBC");
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
            System.out.println("Class not found");
        }
        connection = DriverManager.getConnection("jdbc:sqlite:sqlite_database.db");
//        initialise();
    }


    public void addUser(String fName, String lName) throws SQLException {
        checkConnection();
        createUserStatement(fName, lName);
    }


    private void createUserStatement(String fName, String lName) throws SQLException {
        PreparedStatement preparedStatement = connection.prepareStatement("INSERT INTO users values(?,?,?);");
        preparedStatement.setString(2, fName);
        preparedStatement.setString(3, lName);
        preparedStatement.execute();
    }

    public void updateDeviceTemp(String username, String password, Integer deviceId, Integer targetTemp, Integer realTemp) throws SQLException {
        ArrayList<Device> result = new ArrayList<>();
        checkConnection();
        ResultSet devices = connection.createStatement().executeQuery(String.format(
                "SELECT deviceId, name, targetTemperature, realTemperature  FROM " +
                        "(SELECT * FROM Users JOIN UserDevice ON Users.id = userId JOIN Devices ON deviceId = Devices.id)" +
                        "WHERE username='%s' AND password='%s' AND deviceId='%s'",
                username, password, deviceId));

        if (devices.next()) {
            connection.prepareStatement(
                    String.format(
                            "UPDATE Devices SET %s %s WHERE id = %s",
                            targetTemp == null ? "" : "targetTemperature = " + String.valueOf(targetTemp),
                            realTemp == null ? "" : "realTemperature = " + String.valueOf(realTemp),
                            deviceId))
                    .execute();
        }
    }


    //    private void initialise() throws SQLException {
//        if (!hasData) {
//            hasData = true;
//            Statement statement = connection.createStatement();
//            ResultSet resultSet = statement.executeQuery("SELECT username FROM sqlite_master WHERE type='table' AND username='user' ");
//            if (!resultSet.next()) {
//                System.out.println("Building user table with pre populated values");
//                connection.createStatement().execute("CREATE TABLE user(id integer, " +
//                        "fName varchar(60), " +
//                        "lName varchar(60), " +
//                        "primary key(id));");   // Створюємо таблицю
//
//                 Створюємо тестові дані
//                createUserStatement("John", "McNeil");
//                createUserStatement("Kelly", "McNelly");
//                createUserStatement("Sam", "Kelling");
//                return;
//
//            }
//
//        }
//    }
//
}
