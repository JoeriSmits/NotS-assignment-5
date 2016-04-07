# NotS-assignment-5
## Algemene beschrijving van de applicatie
De applicatie is een load balancer. De applicatie verdeelt het aantal requests over de servers die staan aangegeven in de server lijst.
Er is keuze uit 5 algoritmes. Deze kunnen worden geselecteerd door de combobox aan te passen.
Nieuwe servers kunnen worden toegevoegd en wanneer de gebruiker een server selecteerd in de server lijst en op delete drukt verwijdert.  

De algoritmes zijn:
* Round Robin
* Random
* Load
* Cookie Based
* Session Based

#### Round Robin
Het algoritme gaat het lijstje servers af en begint bij server 0. Ieder request pakt het algoritme de volgende server tot het algoritme bij de laatste server is waar het weer bij 0 begint.

#### Random
Het algoritme pakt een willekeurige server uit de serverlijst.

#### Load
Het algoritme kijkt naar de requests van iedere server die op dit moment worden afgehandeld. De gebruiker wordt gekoppeld aan de server met het minste requests.

#### Cookie Based
De server stuurt een cookie naar de gebruiker met een MD5 hash. Deze hash koppeld de gebruiker aan één specifieke server voor stateful.

#### Session Based
Het algoritme begint met een round robin algoritme totdat de server een session tegenkomt. Wanneer dit zo is wordt dit session id opgeslagen en wordt het session id gekoppeld aan de server. Hierdoor blijft de gebruiker bij één specifieke server voor stateful.

### Architectuur diagram van de applicatie
![Architectuur diagram](https://github.com/JoeriSmits/NotS-assignment-5/blob/master/LoadBalancer.png "Load balancer Architectuur Diagram")

## Verdeling van load over HTTP‐Servers
### Beschrijving van concept
Verdeling van load (Load Balancing) is een techniek om het HTTP verkeer te verdelen tussen verschillende servers.
De techniek is handig bij het gebruik van web servers om zo de betrouwbaarheid te verhogen, of een groter aanbod aan te kunnen. Ook kan men door middel van deze techniek een machine uitzetten en hieraan werken zonder dat de gebruiker dit merkt.

### Code voorbeelden
```cs
// Determine what server we need to connect to
                var selectedServer = _server.GetConnectionInfo(cookie, session);

                // Try to connect to the chosen server
                try
                {
                    TcpClient client = new TcpClient(selectedServer[0], Int32.Parse(selectedServer[1]));
                    var stream = client.GetStream();
```

### Alternatieven
Door bijvoorbeeld gebruik te maken van serviceworker kan je de web applicatie locaal bij de gebruiker hosten. Enige requests die de gebruiker doet worden (als niet zijn gecached) opgeslagen en uitgevoerd wanneer er weer connectie is met een server. Hierdoor hoeft de gebruikerservaring niet beschadigd te worden tijdens momenten van veel load.

### Authentieke en gezaghebbende bronnen
Meijden, A. V. (2002, May 22). Loadbalancing bij Tweakers.net. Retrieved April 07, 2016, from http://tweakers.net/reviews/301/loadbalancing-bij-tweakers-punt-net.html

## Session Persistence implementatie, keuzes en algoritmiek
### Beschrijving van concept
Session persistence is het versturen van het HTTP request naar dezelfde server wanneer er een sessie actief is voor een taak of bijvoorbeeld een transactie.

### Code voorbeelden
```cs
var sessions = Algoritme.sessionsPerServer;

                    // If there is no session set yet it will choose a server based on Round Robin
                    if (session == null)
                    {
                        serverChosen = Algoritme.roundRobinPos;
                        if (Algoritme.roundRobinPos != servers.Count - 1)
                        {
                            Algoritme.roundRobinPos++;
                        }
                        else
                        {
                            Algoritme.roundRobinPos = 0;
                        }
                    }
                    // There is a session
                    else
                    {
                        // Check if the session is already stored in our list of stored sessions
                        var isAlreadyStored = false;
                        object selectedServer = null;

                        foreach (var stringse in Algoritme.sessionsPerServer)
                        {
                            if (session == stringse[0])
                            {
                                isAlreadyStored = true;
                                selectedServer = stringse[1];
                            }
                        }

                        if (isAlreadyStored)
                        {
                            // The session is already stored
                            var l = 0;
                            foreach (var server in servers)
                            {
                                if (server == selectedServer)
                                {
                                    serverChosen = l;
                                }
                                l++;
                            }
                        }
                        else
                        {
                            // Session is not stored yet
                            // Add new session id to the local storage
                            var k = 0;
                            string[] sessionPerServer = null;
                            if (Algoritme.roundRobinPos != 0)
                            {
                                Algoritme.roundRobinPos--;
                            }
                            else
                            {
                                Algoritme.roundRobinPos = servers.Count - 1;
                            }
                        
                            foreach (var server in servers)
                            {
                                if (k == Algoritme.roundRobinPos)
                                {
                                    sessionPerServer = new string[] {session, server.ToString()};
                                }
                                k++;
                            }
                            serverChosen = Algoritme.roundRobinPos;
                            Algoritme.sessionsPerServer.Add(sessionPerServer); 
                        }

```

### Alternatieven
Een alternatief zou kunnen zijn om een cookie te zetten wanneer de situatie stateful moet zijn. Hierdoor kan je een cookie aan een server address of id koppelen en zo session persistence behouden.

### Authentieke en gezaghebbende bronnen
NGINX. (n.d.). What is Session Persistence? Retrieved April 07, 2016, from https://www.nginx.com/resources/glossary/session-persistence/



