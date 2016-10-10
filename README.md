# Executing the sample
Once started, go to http://localhost:9000/swagger for an interactive console

## From Visual Studio
1. Load into Visual Studio
2. Build and run as normal

## From Command Line
(Note: you must have nuget command line on your path)
1. Fire up a Visual Studio 2015 command prompt
2. Excute buildandrun.cmd.

# Notes on implementation
## Philosophy
The patterns here (dividing presentation, domain, and infrastructure) are focused on DDD using patterns described in Vaughn Vernon's "Implementing Domain Driven Design."  The split-out may be a bit much for the current requirements, but it grows well and doesn't really create any additional code.  I did skip implementing any "domain services" as the controller stayed simple enough that I couldn't justify the extra layer.
## What's missing
The following items are not implemented, due to time constraints:
* Authentication - I'd do this with an implementation of AuthenticateFilter, using the standard "Authenticate" header.  If starting from scratch I like to use tokens based on JWT.
* Logging and performance metrics
* Integrate autofac with unit tests - I like to do this for more substantial solutions so tests don't need to encapsulate all the construction details.  Setup can get a bit complex and it's overkill for current size.
* Integration tests
* Build, test and deploy automation


# Notes on testing
* Nowadays I'm accustomed to writing BDD-style tests that exercise an aggregate of functionality rather than fine-grained TDD-style tests.  I'm OK with doing TDD though if that's your style.
* I'm skipping tests that prove model validation.  I could use [this technique](https://magedfarag.wordpress.com/2012/10/17/unit-testing-mvc-controllers-with-model-validation/) to exercise this, but it breaks encapsulation.  The best way is probably to setup the mocks and then set up self-hosting from within the unit tests.  That said, given the risk/complexity tradeoffs, saving these for integration tests isn't crazy either.