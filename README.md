# SOA
Pokretanje projekta:
Desni klik na dockercompose i izaberemo open in terminal
U terminalu unesemo komandu za pokretanje svih kontejnera docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
Za pokretanje Dashboard-a potrebno je pozicionirati se na putanju /src/Dashboard/main i u okviru nje pokrenuti index.html fajl preko live-servera

Glavna ideja sistema je da na osnovu vrednosti ocitanih sa senzora (pm10, pm25, ozone, so2, co) analiziraju vrednosti i utvrdi kojoj zoni zagadjenosti (green, yellow, red) pripada ocitana vrednost i da se na osnovu toga izvrsi odgovarajuca komanda. Od tehnologija koriscen je .NET CORE za implementaciju mikroservisa i čist javascript za implementaciju grafickog interfejsa.

Arhitektura aplikacije i kratak opis mikroservisa

Sensor Device Microservice
Device mikroservis čita podatke sa gore pomenutih 5 senzora i prosleđuje ih Data mikroservisu putem http post zahteva. Čitanje podataka sa senzora simulira se periodičnim čitanjem podaka iz fajla. Moguće je podešavanje intervala (perioda ocitavanja nove vrednosti) i thresholda(za koliko procenta je potrebno da se promeni nova vrednost u odnosu na prethodnu da bi bila poslata Data mikroservisu). Takodje, sadrzi funkcije za pokretanje i stopiranje senzora.

Data Microservice
Data mikroservis prima podatke od Device mikroservisa, upisuje ih u sopstvenu bazu (mongoDB) i prosleđuje u Analytics mikroservis putem brokera (hivemq).

Analytics Microservice
Analytics mikroservis prima podatke sa Data mikroservisa, analizira ih u cilju detektovanja trenutnog stanja na osnovu parametara koji mogu uticati na promenu thresholda/intervala određenog senzora (u zavisnosti u kojoj od zona se nalazi ocitana vrednost). Takođe ovaj mikroservis vrši upis analiziranih podataka u sopstvenu bazu bazu (mongoDB) i šalje analizirane podatke Command mikroservisu putem brokera.

Command Microservice
Command mikroservis dobija podatke od Analytics mikroservisa preko brokera i u zavisnoti od analiziranih podataka (u zavisnosti od zone zagadjenosti) vrsi promenu thresholda/intervala određenog senzora na Device mikroservisu putem http-a. Takođe, ukoliko je noć ugasiće sve senzore. Prosleđuje interfejsu aplikacije izvršenu akciju preko SignalR-a.

Api Gateway
Gateway mikroservis predstavlja REST API za veb klijenta.

Dashboard
Dashboard predstavlja grafički interfejs ove aplikacije, nudi prikaz analiziranih podataka koji su došli do command mikroservisa, kao i opciju za brisanje svih podataka. Prikazuje obaveštenje za svaki novi podatak koji stigne.

Komunikacije putem brokera
Za slanje poruka web klijentu od strane command mikroservisa koristi se SignalR biblioteka za web socket komunikaciju. Za komunikaciju između Data, Analytics i Command mikroservisa koristi se hiveMQ. U aplikaciji postoje dva topic-a, jedan za komunikaciju izmedju Data i Analytics mikroservisa i drugi za komunikaciju izmedju Analytics i Command mikroservisa.
