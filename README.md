# Generic-model-to-assign-XML

It's a piece of code for the generic model to assign XML values and passing them to DatabaseServer!

I had a similar scenario (in one of my projects), reading the XML data and checking and manipulating before running the migrations to the Database Server 
   with the object that contians these values!
             
Suppose that:
 *1  you have more columns on that file than you actually need --- reffer to: arrProperty2Check
 *2  you are itterating in that file, each rows then each columns and you have to assign values to the dictionary --- arrPropertyCommingFromAnotherSource

 *** In this way you have passed the way around to itterate in XML rows and columns, 
    assigning values each of strings above and finally writing the hardcoded object 
    as key pair to pass at the Insert statment of any Database wrapper you're using! ***
