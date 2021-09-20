# BlockSearch
Blockchain Transaction Search

## Setup and Running

### Setup
Before running the solution, please update the ``appsettings.json`` file, to include the desired projectId in the ``NethereumClient`` section.

I wasn't sure of the sensitivity surrounding the projectId, so I left it out.


### Running
The easiest way to build, run, and debug this solution is to install Microsoft VisualStudio, and run it in the IDE.

It may be necessary to install .Net Core on your machine (if not already), to get the solution running in Visual Studio.

If running as a docker container, it may be necessary to install Docker Desktop for your machine (if not already).

To open the solution, select the ``BlockSearch.sln`` file in VisualStudio.


### Running Options
Within Visual Studio, the solution can be debugged as a .NetCore app, with ``BlockSearch.MVC`` selected as the start-up project, or within a docker container. Simply toggle the desired option in the main menu bar within Visual Studio.

Running the solution in VisualStudio will open a browser, which should land straight on to the ``/TransactionSearch/`` page. 

### BLockSearch Solution

The solution is divided into numerous projects, each dedicated to a particular concern.

The projects are: 

1. BlockSearch.MVC

2. BlockSearch.Application

3. BlockSearch.Common

4. BlockSearch.Infrastructure


#### BlockSearch.MVC

The BlockSearch.MVC project effectively handles the presentation layer, taking input from the user - in this case, via a (very) simple razor page.

It was put together from the standard 'out-of-the-box' MVC solution, and the TransactionSearch page was added.

The TransactionSearch page is a basic form that accepts a BlockNumber (integer) and an optional Ethereum Address to filter on.

The existing page and controller are hard-coded to perform Ethereum searches, however with some tweaking (front and back-end) it could be extended to work with other similar crypto blockchains.


#### BlockSearch.Application

The BlockSearch.Application project contains the solution's business logic, as well as the various internal services and external clients use to fetch block and transaction data.

The main entry-point into the BlockSearch functionality is through the BlockSearchService. 

Based on the Crypto type requested (currently only Ethereum), it uses a factory class to load up the appropriate crypto service to process the request.

Using a factory class to call up specific Crypto Service implementations, allows the BlockSearchService to remain largely unchanged if a new Crypto Service was to be added with the same functionality.


#### BlockSearch.Common

The BlockSearch.Common project contains all the classes shared between the MVC and Application projects, being mostly domain models.


#### BlockSearch.Infrastructure

The BlockSearch.Infrastructure contains the Logger implementation, as well as an options class to dependency inject connection options into the external client that connects to the Infura Ethereum service.


## Git Commits
As a voluntary measure for sanity testing, and to keep the repository free of bugs, please copy the ``pre-commit`` file from the ``GitCommitScripts`` folder to the ``.git/hooks`` folder to trigger the Release build and unit tests to run, preventing erroneous code from being commited.


## Assumptions
Several assumptions were made in creating this solution.

1. For the purposes of keeping this exercise simpler, authentication and authorization were assumed to not be required, therefore each request is assumed to be valid for the consumer.

2. Minimal logging was implemented, as there was no specific requirement.


## Production - Things to consider

I would consider several options for a production environment:

1. Although I attempted to satisfy some of the risk concerns with the pre-commit hook, it should go without saying that in a production setting, it would be necessary to run those build checks and run the unit tests in a proper deployment pipeline, as a gate-keeper for deployment to any environment. 

2. Given the light-weight nature of the solution (in particular, no database with no Entity Framework models to deal with), I would consider an AWS Lambda (or Azure Functions equiavalent) sitting behind an API Gateway, to run it in production.

3. As the service could be used to flood requests to the Infura APIs, I would give consideration to rate-limiting this service.

4. Additional logging of incoming requests and requests to the downstream APIs, to provide increased visibilty in case of errors that arise.