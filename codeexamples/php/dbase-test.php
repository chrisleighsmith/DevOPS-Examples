<?php
$servername = "nusa";
$username = "db";
$password = "SMT";

// Create connection
$conn = mysqli_connect($servername, $username, $password);

// Check connection
if (!$conn) {
    die("Connection failed: " . mysqli_connect_error());
}

echo "Connected successfully";
echo " ";
 
$sql = "show databases";

$result = $conn->query($sql);
 
if ($result->num_rows > 0) {
    // output data of each row
    while($row = $result->fetch_assoc()) {
//        echo "id: " . $row["id"]. " - Name: " . $row["firstname"]. " " . $row["lastname"]. "<br>";
        echo "Database: " . $row["Database"]. "<br>";
    }
} else {
    echo "0 results";
}
$conn->close();
?>
