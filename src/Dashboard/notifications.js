import { connection } from "./signalr.js";

export class Notifications
{
    constructor(parent)
    {
        this.notifications = [];
        this.container = null;
        connection.on("SendEvent", data => {
            console.log(data);
            this.addData(data);
            this.draw(parent);
          });
    }

    addData(newData)
    {
        this.notifications.push(newData);
    }

    draw(parent){

        function createDiv(parentDiv, className, textContent)
        {
            var div = document.createElement("div");
            div.className = className;
            div.textContent = textContent;
            parentDiv.appendChild(div);
        }

        this.container = document.createElement("div");
        this.container.className = "notificationsDiv";
        parent.appendChild(this.container);
        var div = this.container;
        this.notifications.forEach(element => {
            createDiv(div, "notificationDiv", element);
        });

    }
}