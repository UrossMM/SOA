//Object.defineProperty(WebSocket, 'OPEN', { value: 1, });

var connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:8003/commandhub", 
  {    
    skipNegotiation : true,
    transport: signalR.HttpTransportType.WebSockets
    
    })
  .configureLogging(signalR.LogLevel.Trace)
  .build();

  connection.on("ReceivedMsg", data => {
    console.log(data);
  });

connection.start();

// connection.onreconnected(() => {
//   apiFetch('POST', `http://localhost:32610/api/ServiceCommand/Subscribe/?connectionId=${connection.connectionId}`);
// });

export { connection as connection};