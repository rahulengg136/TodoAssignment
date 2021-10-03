# TodoAssignment 

</br>The solution contains 3 different layers - API, Business and Data. The database is there in the data layer as mdf file. Only changing the "AttachDbFilename" value in appsettings.Development.Json will work.

</br>End points:  There are 4 end points
              </br>-> Login
              </br>-> Label
             </br> --> TodoItem
             </br> --> TodoList
              
   </br>Steps to run project 
   </br>--> Take clone
   </br>--> Change the connection string's "AttachDbFilename" key according to mdf file(in DAL project) location in your system. Please see existing connection string properly before          changing this.
   </br>-->Run the project, will be able to see swagger UI
   </br>-->Hit Login controller with below body
</br>{
  </br>"userName": "rahul",
  </br>"password": "rahul123"
</br>}
   </br>-->Take the token from response and put it in authorize section pop up that comes after clicking Authorize button
   </br>-->Will have access to all end points now. Just need to provide correct required info. Otherwise, may get bad requests

              
              
