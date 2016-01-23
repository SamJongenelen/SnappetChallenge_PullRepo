
**Project requirements and info**

***Requirements***

- Het project is geconfigureerd om Local IIS te draaien. 
- Visual Studio 2015, of iig .net framework 4.6 ``(let op compiler versie, ik gebruik c# 6.0 syntax operators (nameof(), string helpers))``;
- Front end is een ASPNET MVC Project (csproj WebApplication1)
 *  Project bevat zowel Controllers voor posten/get als Razor Views 
- NuGet door onder andere EF, jQUery & Bootstrap. Gebruik evt 'nuget.exe restore contoso.sln', of gewoon VS :)
- Service/Data laag (klein gehouden omdat het een PoC is) is WebApplication1.Data
 * Bevat EF logica, zoals DBContext, Entities en repositories om de entities te managen.


** Hosted environment demo **

http://webapplicationsnappet.azurewebsites.net/

Gekoppeld aan de Snappet GIT branch met CI. Indien CI slaagt wordt Release gestart. _Zie onderstaand_



** Process management met TFS demo **

Voor een demo van mijn Team Foundation Services (voorheen VSO) account setup kan ik je uitnodigen in mijn 'team'; zodat je mijn CI builds en Continious integration setup kunt zien.
Het is GIT repository in TFS, met een Master branch en een Snappet Branch. De CI zit gekoppeld aan de Snappet branch checkin, waardoor een build start. 
Als de MSBuild Actions en de VsTest Engine run slagen, wordt een _Artifact_ (zip) gekoppeld aan deze build en een Release Definition afgevuurt.
Deze Release connect middels username + certificate credential naar mijn Snappet Web App, hosted op Azure.
Ik heb voor de grap gewerkt met een TFS backlog en work items, om zo ook te kijken wat de voortgang is van TeamFoundationServices, ten opzichte van de door mij beheerde TFS On-Premises 2015 update 1 installatie. 


Dan nu verder over de code; 

De sln bestaat uit 3 projects; een UnitTest project, om aan te geven dat testen belangrijk is 



Data laag is een EF 6.1.3 code first approach, zonder database achter de context, zodat deployment wat makkelijker gaat.
-- Ook heeft mijn Azure account helaas net niet genoeg credits om een WebApp + SQL Server instance te draaien :)



