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
1) Done - Start tracking the full AirshowGroup globally because the Airshow class doesn't hold that value.
2) Done - expose a way to change the YeaarOfInterest.
3) done - Fixed the issue - get rid of toolTip1
4) Done - in MS (Middle South) group Texas, Oklahoma and New Mexico to their own region
5) Done - Added Data Base Management -> Update Additional Fields not all performers are showing up - Yuma for example
6) add an email field to contacts
7) add number of days the show is to the text summary in the airshows by weekend.
8) mexico airshows
9) add a contact database that is seperate from the airshow list and pull in the contacts based on location. 
10) add a database of contact respoonses etc. 
11) make a custom form to show contacts in the airshow viewer. stop using the freebie forms
12) highlight airshows that showup mid week with a funny color so they don't block the weekend.
13) make the notes thing work better so we caan keep track of exchanges. 
14) ICAS furniture, 2 tall round table with 4 chairs.
15) Done - order a bad ass mac that can run parallels and has enough memory. 
16) Paris Hotel
17) Clean up hotel reservation. 
18) red and orange drop cloth for tables
19) rental car receipt for UA.

# coding things
# ColorGrid: 
 - adds text to the gridview if an airshow has a status that is interesting
 - bolds the text in cells where the filtered list has an airshow that weekend
 
 things to know:
 - anytime you change the database, you need to call ColorGrid with the complete airshow list.  We assume that anytime the database is saved, that there is a change and we pass myAirshows
 - then we run it again to apply myFilteredAirshows


 #LoadGrid
 - this fills the grid with an AirshowWeekEnd, which is holds a System.Date.Time member an its ToString is displayed in the cell.
 - this should only need to be done when the YearOfInterest changes.

