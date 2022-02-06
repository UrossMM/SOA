export class Data{
    
    constructor(id, type, city, value){
        this.id = id;
        this.value = value;
        this.type = type;
        this.city = city
        this.container = null;
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
        this.container.className = "parameterDiv" + this.id;
        parent.appendChild(this.container);

        createDiv(this.container, "parameterField", "Sensor Type: " + this.type);
        createDiv(this.container, "parameterField", "City: " + this.city);
        createDiv(this.container, "parameterField", "Value: "+ this.value);

    }
}