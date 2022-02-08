
var connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:8003/commandhub", 
  {    
    skipNegotiation : true,
    transport: signalR.HttpTransportType.WebSockets
    
    })
  .configureLogging(signalR.LogLevel.Trace)
  .build();

  connection.on("ReceivedMsg", data => {
    swal("Warning!", data);
  });

connection.start();

export { connection as connection};