# AirshowSchedule
## Display sizes

The calendar and show/filter controls sit side by side on wide windows and stack on narrow or portrait windows. The legend and action buttons wrap, and the calendar scrolls when its cells cannot fit. Windows use per-monitor DPI scaling. Older editors scroll when the screen is smaller than their content, and dialogs fit the current monitor's working area.

The advanced calendar import command uses a resizable file/year/source dialog instead of the legacy generated settings UI. Existing XML databases and saved import settings retain their format and location.

Run `dotnet run --project tools/LayoutCheck` for window layout checks. Manually check dragging the app between monitors at different Windows display scaling settings.

## Calendar and planning

The calendar shows Saturday–Sunday dates, month labels, show counts, and a status-color legend. Events appear on every weekend their dates overlap, including Sunday-only events. Colors indicate contract, verbal, maybe, or pursue status. Contract and verbal commitments remain visible across all regions even when the region filter hides their shows; hover for full names and dates. Clicking a weekend lists its filtered weekend events. Weekday-only events have a separate count and **Weekday shows…** view and do not color or occupy a weekend. Unmapped locations are available under **Other / international**.

Choose **Needs attention…** for upcoming shows in the selected year across all regions. It flags cancellation warnings, overlapping contract/verbal commitments, missing phone/email details, and shows marked Pursue. Applied ICAS update history also supplies missing-show warnings and changes to booked dates, including date changes not accepted during review. Date warnings remain available after subsequent imports. Use **Include past shows** for earlier events.

Open a show from the attention list to edit it. **Mark reviewed** hides that individual warning without changing the show or its booking status. **Show reviewed** lets you reopen a warning. If its details change, it appears again. Review acknowledgments are stored in an `.attention-*.json` file beside the active database, scoped to the selected show/contact file pair; keep this file with the database. Existing notes do not establish whether a follow-up happened, so Pursue flags ask you to review the next step rather than claiming a contact is overdue.

## Updating from ICAS

Choose **Update from ICAS…** on the main menu and select the downloaded calendar. The calendar year is detected automatically. Review the proposed changes, use the category filter, and check the fields you want to apply. Existing contact replacements start unchecked. Missing and ambiguous events are review-only; cancellation warnings preserve your booking status. Applying changes saves both active databases and refreshes the calendar. No intermediate export is necessary.

Each applied update stores the original download, before/after copies of both databases, and the selected changes beside the active show database in `<database filename>.icas-history`. Use **File → ICAS update history…** to inspect these records. **File → Undo last ICAS update…** restores both files exactly, provided no subsequent edits have been saved. An interrupted update or undo can be recovered using the same Undo command. Keep the history folder with the database.

Reimporting an applied download produces no further changes for fields already accepted. Fields you declined remain available to review. Notes, links, booking statuses, and existing date-added values are preserved. Blank incoming contacts do not erase your contact links. Shared contacts are preserved when a show's contact details change.

Legacy parse, export and compare commands are under **File → Advanced import / export**.

# Converting ICAS Data for Use with This Tool

1. Go to airshows.aero
2. Sign in
3. Download the calendar for the year of interest
4. Parse Data File (File -> Parse Data File) and select the downloaded .doc directly. ICAS currently exports an HTML table with a .doc extension; binary Word .doc and .docx files are not supported. Legacy text exports remain supported.
5. Set the year to match the downloaded calendar. Review the imported show/contact counts; malformed tables produce an error.
6. Keep the parsed import separate from your active database until you compare and review it. Cancelled labels remain in show names; they do not automatically change your booking status.
7. Save-as yyyy-mm-dd+"filename".asg.xml (file->Save Parsed Data File)

# Updating Database

1. Compare XML Files (Data Base Management -> Compare To Active DB)
2. Choose the active DB
3. Choose the file you want to compare
4. Select the dates you'd like to add

# cleaning up the database

Choose **Data Base Management → Clean up database…** and select the latest comparison `.asg.xml` once. Its matching `.contacts.json` is loaded alongside it; if it is missing, select the corresponding contacts file. The workflow uses the comparison database's year and reviews events in date order.

1. **Clean up DB:** review possible same-location duplicates and events missing from the comparison database. Check only shows to remove; separate events can share a location and missing events may have moved dates.
2. **Check for cancelled shows:** review cancellation warnings from the active and comparison databases. Removals start unchecked, including booked shows.
3. **Update additional fields:** unique location/year/week matches start checked. For ambiguous matches, choose at most one comparison show per active show. Dates and booking statuses are preserved.
4. Review the actions and choose **Apply changes** to save both active databases together. **Skip this step** advances without its changes. **Cancel** discards all staged changes, including earlier steps.

Both original files and the action list are saved beside the show database in `<database filename>.cleanup-history`. **Undo last cleanup…** restores both files exactly if no subsequent changes have been saved; it can also recover an interrupted cleanup. Keep this folder with the database. The former one-time contact-extraction migration menu has been removed; current imports already create separate contact files.

# things to do:
1) highlight airshows that showup mid week with a funny color so they don't block the weekend.


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
