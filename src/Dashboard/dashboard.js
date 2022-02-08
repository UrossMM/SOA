import { Data } from "./data.js";

export class Dashboard
{
    constructor()
    {
        this.data = [];
        this.container = null;
    }

    addData(newData)
    {
        this.data.push(newData);
    }

    deleteAll()
    {
        this.data = [];
    }

    draw(parent){

        var dash = this;

        var text = document.createElement("h2")
        text.textContent = "Air Pol Delhi"
        text.style.textAlign = "center"
        parent.appendChild(text);

        this.container = document.createElement("div");
        this.container.className = "parameterDiv";
        parent.appendChild(this.container);

        var div = document.createElement("div");
        div.className = "header"
        parent.appendChild(div)

        var form = document.createElement("div");
        form.className = "formDiv";
        this.container.appendChild(form);

        var elements = document.createElement("div");
        elements.className = "elementsDiv";
        this.container.appendChild(elements);

        var div1 = document.createElement("div");
        form.appendChild(div1);
        var input1 = document.createElement("input");
        input1.type = "button";
        input1.value = "Delete all";
        input1.className = "deleteAllInput";
        div1.appendChild(input1);
        input1.onclick = function(){
            fetch('http://localhost:8010/Data/RemoveAllData', {
                method: "DELETE",
                headers: {'Accept': 'application/json', 'Access-Control-Allow-Origin': '*'}
                })
                .then(location.reload())
        }

        var div2 = document.createElement("div");
        form.appendChild(div2);
        var input2 = document.createElement("input");
        input2.type = "button";
        input2.value = "Get all";
        input2.className = "getAllInput";
        div2.appendChild(input2);
        input2.onclick = function(){

            dash.deleteAll();
            fetch("http://localhost:8010/Data", {
                method: "GET",
                
                headers: {
                    "Accept": "*/*",
                    "Connection":"keep-alive"
                  }
            }).then(p => p.json().then(data => {
                data.forEach(d => {
                    const newData = new Data(d["id"], d["sensorType"], d["city"], d["value"]);
                    dash.addData(newData);
                });
                dash.drawElements(elements);
            }));
            
        }

        this.drawElements(elements);

    }

    drawElements(elements){
        elements.innerHTML = "";
        this.data.forEach(element => {
            element.draw(elements);
        });
    }
}