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

Running the solution in VisualStudio will open a browser, which should land straight on to the ``/TransactionSearch/`` page. If not, a link to the Transaction Search page was added to the top navigation bar.

#### Docker (Optional)
If you wish to run the solution as a stand-alone docker container outside of Visual Studio, please follow these instructions:

1. Make sure you have updated the ``appsettings.json`` file with your Infura Project ID, as mentioned previously.

2. Open PowerShell and navigate to the solution's root directory (where the BlockSearch.sln file is located).

3. Type ``docker build -t blocksearch-image -f BlockSearch.MVC\Dockerfile .`` and hit enter. 

4. Navigate to the BlockSearch.MVC folder (where the Dockerfile is located) by typing ``cd BlockSearch.MVC`` and hit enter.

5. Type ``docker run -it --rm -e "ASPNETCORE_ENVIRONMENT=Development" -p 58817:80 --name blocksearch-mvc blocksearch-image``, and hit enter. Note: You can replace the port mapping of ``58817:80`` with any of your choosing.

6. The docker container should now be running with the solution. You should be able to visit the site by entering ``http://localhost:58817/TransactionSearch`` into your browser.

7. If you wish to create an image file to copy to another machine, type ``docker save -o blocksearch-image.tar blocksearch-image``, to create a *.tar file.


## BlockSearch Solution

The solution is divided into numerous projects, each dedicated to a particular concern.

The projects are: 

1. BlockSearch.MVC

2. BlockSearch.Application

3. BlockSearch.Common

4. BlockSearch.ExternalClients

5. BlockSearch.Infrastructure


### BlockSearch.MVC
The BlockSearch.MVC project effectively handles the presentation layer, taking input from the user - in this case, via a (very) simple razor page.

It was put together from the standard 'out-of-the-box' MVC solution, and the TransactionSearch page was added.

The TransactionSearch page is a basic form that accepts a BlockNumber (integer) and an optional Ethereum Address to filter on.

The existing controller is hard-coded to perform Ethereum searches, however with some tweaking (front and back-end) it could be extended to work with other similar crypto blockchains.


### BlockSearch.Application
The BlockSearch.Application project exists as the core domain of the 'Block Search' business logic, as well as the various internal services used for processing.

The main entry-point into the BlockSearch functionality is through the BlockSearchService. 

Based on the Crypto type requested (currently only Ethereum), it uses a factory class to load up the appropriate crypto service to process the request.

Using a factory class to call up specific Crypto Service implementations in this context is one means of promoting extensibility, allowing the BlockSearchService to remain largely unchanged, if a new Crypto Service was to be added with the same functionality.


### BlockSearch.Common
The BlockSearch.Common project contains all the classes shared between the MVC and Application projects, being mostly domain models and exceptions.


### BlockSearch.ExternalClients
The BlockSearch.ExternalClients project contains the clients dedicated to reaching to outside services to fetch block and transaction data. In the case of the current client that is implemented, it is a client dedicate do communicating with the Infura service, using Nethereum Nuget package. 


### BlockSearch.Infrastructure
The BlockSearch.Infrastructure contains the Logger implementation, as well as an Options class to dependency inject connection options into the external client that connects to the Infura Ethereum service.


## Git Commits
As a voluntary measure for sanity testing, and to keep the repository free of bugs, please copy the ``pre-commit`` file from the ``GitCommitScripts`` folder to the ``.git/hooks`` folder. On each commit, this hook will trigger the Release build, and run unit tests, preventing erroneous code from being commited.


## Assumptions & Notes
Several assumptions were made in creating this solution.

1. For the purposes of keeping this exercise simpler, authentication and authorization were assumed to not be required, therefore each request is assumed to be valid for the consumer.

2. Minimal logging was implemented, as there was no specific requirement.

3. There was no requirement that would exclude the use of third-party libraries to perform key parts of the exercise. The Nethereum Nuget package comes to mind, and warrants a special mention as it was particularly helpful.

4. The front-end razor view was built as minimally as possible (with minimal consideration to presentation), as it was assumed the focus of the exercise was to demonstrate back-end code writing, structure, testing, and documentation.

5. Where relevant, code sourced from other people's work was referenced accordingly.


## Production - Things to consider

I would consider several options for a production environment:

1. Although I attempted to satisfy some of the risk concerns with the pre-commit hook, it should go without saying that in a production setting, it would be necessary to run those build checks and run the unit tests in a proper deployment pipeline, as a gate-keeper for deployment to any environment. 

2. Given the light-weight nature of the solution (in particular, no database with no Entity Framework models to deal with), I would consider an AWS Lambda (or Azure Functions equiavalent) sitting behind an API Gateway, to run it in production.

3. As the service could be used to flood requests to the Infura APIs, I would give consideration to rate-limiting this service.

4. Additional logging of incoming requests and requests to the downstream APIs, to provide increased visibilty in case of errors that arise.