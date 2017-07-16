# csv-to-json-for-testing
Arbitrary CSV parser for practicing unit testing.

All,

As discussed here is intern project that should give some exposure to DotNet Core and give something not trivial to practice good unit testing on.
This will take place in two main parts:
1)	Write a DotNet Core project (described below) with no unit testing
2)	Go back and try to unit test the project, likely having to do refactoring to allow for testing

Throughout this process we can and will have formal and informal meetings where necessary to help where needed.
Shawn, Trevor, and Ben feel free to write the project as well or if you are mostly interested in the conversations on unit testing I will write the project and you can just come to the unit testing meetings and follow along with my project.

Without further ado…

The Project

Write a DotNet Core 1.1 console app that parses an arbitrary CSV file and outputs in JSON format.
Acceptance Criteria:
1)	Reads from an arbitrary CSV file
a.	The path to the CSV file is passed as a command line argument
b.	The first row of the CSV file is assumed to be the column headers and map to the keys in JSON output.
2)	Prints the contents as JSON array
a.	Print to standard out
b.	Nice Formatting is not required (but probably helpful)

For Example a file with the contents:
Alpha, Beta, Gamma
Apple, Banana, Grape
Ant, Bat, Gorilla

Should be printed as (with whatever valid JSON formatting):
[
  {
    "Alpha": "Apple",
    "Beta": "Banana",
    "Gamma": "Grape"
  },
  {
    "Alpha": "Ant",
    "Beta": "Bat",
    "Gamma": "Gorilla"
  }
]

Other Rules and considerations:
1.	No libraries to help with CSV or JSON. The point of this exercise is to get practice with DotNet Core and unit testing less trivial code.
2.	Don’t unit test. We will talk about that after everyone finishes their project.
3.	Don’t over complicate the solution or worry about how to unit test it. If all goes well you will make design decisions that will require some serious refactoring to be able to correctly unit test. That’s what I’m hoping for, because that is how we learn.
4.	The CSV parsing can be naive
a.	Assume that that every row has the same number of columns
b.	Don’t worry about escaping commas
c.	Trim whitespace on columns
5.	The JSON formatting can be naive
a.	You don’t have to write a full JSON formatter the output will always follow the same pattern
6.	Deal with errors (i.e missing file) by printing something helpful back to the user and exiting
7.	Feel free to collaborate.
8.	Have Fun and Learn Something!

Let me know if you have questions on the project or anything. I will be doing project right alongside you all.

Thanks,
Josh
