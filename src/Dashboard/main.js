import { Dashboard } from "./dashboard.js";
import { Notifications } from "./notifications.js";

var dashboard = new Dashboard();
dashboard.draw(document.body);

var notifications = new Notifications(document.body);
notifications.draw(document.body);