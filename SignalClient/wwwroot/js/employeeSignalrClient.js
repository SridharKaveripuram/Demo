   

  $(document).ready(function() {

    let connection = null;
    let webSocketUrl = $("#webSocketUrl").val();   
          
    function initializeSignalrClient() {

        connection = new signalR.HubConnectionBuilder()
            .withUrl(webSocketUrl, {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets
            })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

        connection.on("RecieveMessage", (empMessage) => {
        document.getElementById("employeesResult").value += JSON.stringify(empMessage) + "\r\n";
        });          
    }

     async function startConnection() {
        try {
             await connection.start();
             console.log("SignalR Connected.");
             document.getElementById("employeesResult").value = "";
        } catch (err) {
            console.log(err);
            setTimeout(startConnection, 5000);
        }
     };

     async function startButtonClicked() {
        if (!connection) {
           initializeSignalrClient();
        }
        await startConnection();
     }

     async function stopButtonClicked() {
          await connection.stop();
          console.log("Connection stopped");
     }

      $("#stopButton").prop("disabled", true);

      $("#startButton").on("click", function () {
          
          startButtonClicked();
          $("#startButton").prop("disabled", true);
          $("#stopButton").prop("disabled", false);
      });

      $("#stopButton").on("click", function () {
      
          stopButtonClicked();
          $("#startButton").prop("disabled", false);
          $("#stopButton").prop("disabled", true);
      });
  });
