# TodoAssignment 
              
   </br>Steps to run project 
   </br>1.)  Take clone
   </br>2.) Open the solution in visual studio
   </br>3.) Open appsettings.development.json file and change connection string.
   </br>--> Change "AttachDbFilename" key according to mdf file(in DAL project) location in your system. 
   </br>-->Please see   existing connection string properly before changing this. 
   </br>-->Make sure there will maximum 2 "\\" characters in whole connection string not more than that like "\\\\"
   </br>If you double click mdf file in solution explorer, database can be accessed from server explorer.
   </br>4.) Run the project, will be able to see swagger UI
   </br>5.) Hit Login controller with below body
</br>{
  </br>"userName": "rahul",
  </br>"password": "rahul123"
</br>}
   </br>6.) Take the token from response and put it in authorize section pop up that comes after clicking Authorize button
   </br>7.) Will have access to all end points now. Just need to provide correct required info. Otherwise, may get bad requests

              
              
