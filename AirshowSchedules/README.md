# AirshowSchedule
# Converting ICAS Data for Use with This Tool

1. Go to airshows.aero
2. Sign in
3. Download the calendar for the year of interest
4. Open in Word
5. Save as a text file
6. Parse Data File (file->Parse Data File)
7. Save-as yyyy-mm-dd+"filename".asg.xml (file->Save Parsed Data File)

# Updating Database

1. Compare XML Files (Data Base Management -> Compare To Active DB)
2. Choose the active DB
3. Choose the file you want to compare
4. Select the dates you'd like to add

# cleaning up the database

1. (Data Base Management -> Clean Up DB)
2. Initially you'll want to compare to the most recent download.  This will go to the console for user diplay and input. 
3. Once you've done that, run it again and don't compare, to see a list of remaining potential duplicates.  But becareful, we only are comparing locations and some locations have more than one airshow. 

# checking for cancelled shows

1. (Data Base Management -> Check For Cancelled Shows)
2. Follow the prompts.

# things to do:

1. after nuking duplicate shows, notify the user that they are now removing shows not in the new DB
2. move the "do you want to remove these shows prompt below the list or add a seperator
3. at the end of clean up database, tell them it was successful. 
4. after cleaning up the database, the title of some of the grids isn't correct.  investigaate.  this list and filter is right, just not the grid title.
5. copy loadgrid and color grid to saveairshows...not what we want.  when filtering the list, we lose some context.  we only want to LoadGrid on specific times
