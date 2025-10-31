# Bizcord_Chat

## Bizcord design
Startede med at lave et simple klasse diagram over bizcord med tre klasser (User, Message og Channel) 

<img width="1025" height="1008" alt="Bizcord_diagram1" src="https://github.com/user-attachments/assets/622d7f58-ef9a-4854-8a1d-babc2926c35a" />  

Efter det første diagram lavede jeg et større overordnet diagram som viser hvordan forskellige service og ligende dele af systemet kommunikere.    

<img width="5469" height="1696" alt="Bizcord_highlevel_diagram" src="https://github.com/user-attachments/assets/9a895e93-cdb5-440b-8d1d-60f3e8c3f567" />  

På basis af de tegnede diagrammer begyndte jeg at at udvikle bizcord. 
Formålet er at opstille en struktureret løsning med forskellige lag, som helst skal være let forståeligt.  
Lagende jeg vil implementeret er.    
- Toplvel det generelle løsning lag som indeholder de vigtige filer for at et program kan køre. 
- UI-lag det er samlet i projektet Pages mappe.  
- Real tid-lag  
- API-lag  
- Applicatins-lag  
- Data-lag  
- Messaging-lag  
  
Målet er at løsningen i sidste ende skal sende en chatbesked fra en brugers UI via realtid SignalR til (SignalR)Chathubben eller til en messageControlleren.

Hubben eller controlleren kalder derefter IMessageServicen, her valideres beskeden.
  
Beskeden mappes fra et DTO(Data Transfer Object) til entiteten med Automapper pg gemmesderefter med BizcordDBContext. (Arbejder stadig med at få det korrekt implementeret) 
  
Herefter udgiven en domænehændelse gennem IMessagePublisher (RabbitMQ) denn e sender notifikerer, arkivere og opdatere gennem de foskellgie kanaler.  (Arbejder stadig med at få det korrekt implementeret)   
  
Samtidig vil Hubs straks sende beskeden videre til andre tilsluttede klienter for at sikre en lav-latens for eventuelle brugere.  

## Branching strategy and solution setup
Branching stategien er at opdele de forskellige feature til være sin bracnh for hvis man så er nød til at ændre noget senere på projekte DB lag så kan man bruge brachen som refeer til DB laget.  
Så på ens local repo køre man man __git pull origin main__ så det local repo kører på det mest relevante kode.  
Når man så har opdateret det som mangler kan merge den ind i main og få opdatereringer igennem.  
Jeg valgte at de forskellige branches som tilføjer nye feature har prefixet feature/---     
I det tilfælde der opdages et bug som skal fixes skal prefix være bugfix/----  

Jeg startede med at bruge den første branch var havde til mål at opstille en Razorpage løsning sammen med en gitignore fil for at uslade der automatiske som bliver generet når man arbejde med frameworks som genere en bredvifte af filer.  

## 1 Branch feature/Solution-setup af base opjekt filer før start  
The first branch i made where to create the web project which where to use the multitude of services.  
  
## 2 Branch feature/MessageService
After I created the inital web platform I began working on the message service and  
  
## 3 Branch feature/setupDBLayer
Efter jeg havde arbejdet med message service tænkte jeg det ville være smart at tilføje DB delen af lsøsningen for have noget at få APi, og service skulle ramme i sidste ende, så det omhandler DB context filer og appsettings.json.  
  
## 4 Branch feature/AddChannelService
Efter jeg havde færdig gjort min DB opstilling ville gå iganem ed at skrive de forskellige service omhandlende filer.  
  
## 5 Branch feature/AddRabbitMq  
Som titlen på branchen siger så ville jeg inføre brug af RabbitMw i dennem branch som så sendte med at jeg også påsatte en SignalR chathub som skulle fungere som bro mellem RabbitMq og brugerne af Bizcord. 

## 6. Branch feature/Add_API_Gateway

målet med denne branch er at tilføje en api gateway med ocelot. 
Hvorfor?:

Hvordan?: 
## Mangler   
Jeg har endnu ikke fået total flow igennem og det giver lidt problemer for syntes det er letter at tilføje det til sidst så man lette kan se om det køre korrekts.
Så der mangler funionelt docker flow med compose.  
Jeg burde nok også dele det bedre kan se når jeg kigge min Branch historik at commits carier i størrelse og tanker det ville være bedre at lave flere også bare gøre dem mindre.  
