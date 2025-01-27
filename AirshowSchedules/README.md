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
4. You may have deleted an airshow by accident, so re-run (Data Base Management -> Compare To Active DB)

# checking for cancelled shows

1. (Data Base Management -> Check For Cancelled Shows)
2. Follow the prompts.
3. You may have deleted an airshow by accident, so re-run (Data Base Management -> Compare To Active DB)

# things to do:
12) highlight airshows that showup mid week with a funny color so they don't block the weekend.


# coding things
# ColorGrid: 
 - adds text to the gridview if an airshow has a status that is interesting
 - bolds the text in cells where the filtered list has an airshow that weekend
 
 # things to know:
 - anytime you change the database, you need to call ColorGrid with the complete airshow list.  We assume that anytime the database is saved, that there is a change and we pass myAirshows
 - then we run it again to apply myFilteredAirshows


 # LoadGrid
 - this fills the grid with an AirshowWeekEnd, which holds a System.Date.Time member an its ToString is displayed in the cell.
 - this should only need to be done when the YearOfInterest changes.

# extracting contacts thoughts
 - assign id's to each airshow
 - extract contacts
 - remove unnecessary contact information from shows
 - figure out how to load everything