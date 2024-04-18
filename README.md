# CFeedback
Project MVC 5 - .Net core 8 | Database: My Sql Server

<b>Instructions</b> <br/>

1- Clone the project
<br/>
2- Go to appsettings change the connection string to point to your DB
<br/>
3- Run the script "update-database" to update your database (default information will be inserted into the database for testing)
<br/>
4- Run the application
<br/>

<b>A brief explanation of the design pattern used and why.</b> <br/>
<li>
  Basic repository pattern.
</li>
<br/>

<b>Any assumptions or design decisions made during the development process.</b> <br/>
<li>
  There was only a need for the CRUD of Feedbacks, I assumed there was a Category table, I did create this one due to the need for displying the category name. But I'm asuming there is another app that take else of everything else.
</li>
<li>
  Implemented an Ajax call only for the "last month filter", I saw no need for any other functunality, due that Razor adapts nicely to it.
</li>
<br/>

<b>Documentation of branch management strategies used during development.</b> <br/>
It is just me doing the development, so just master and my dev branch.
<br/>

<b>Describe in the submission document any additional security measures you recommend for a production environment</b> <br/>
<ul>
  <li>
    Authorization and Authentication, if this is an application which its only responsability is the maintenance for Feedbacks, I might assume there is another application that handles a centralized Authentication, in which case I would create a new login pointing to such system and adapting the Authorization to its roles.
  </li>
  <li>
    Use SSL and HTTPS
  </li>
</ul>
