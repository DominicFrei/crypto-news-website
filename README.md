# Building a Crypto News Website using the Microsoft Azure App Service and MongoDB Atlas

Who said creating a website has to be hard? Writing the code, persisting news, hosting the website.
A decade ago this might have been a lot of work. These days, thanks to
[Microsoft Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor),
[Microsoft Azure App Service](https://azure.microsoft.com/en-us/products/app-service/#overview)
and [MongoDB Atlas](https://www.mongodb.com/atlas/database) you can get started in minutes.

In this tutorial I will walk your through setting up a new Blazor project, creating a new page with a simple UI,
creating data in Atlas, showing those news on the website and making the website available by using Azure App Service
to host it.

Everything you need will be in this tutorial but if you prefer to just read along for now, check out the [GitHub
repository for this tutorial](https://github.com/mongodb-developer/crypto-news-website) where you can find the code
and the tutorial.

## Prerequisites for this tutorial

Before we get started, here is a list of everything you need while working through the tutorial.
I recommend getting everything set up first so that you can seamlessly follow along.

* [Download and install the .NET framework](https://dotnet.microsoft.com/en-us/download).
  For this tutorial I am using .NET 7.0.102 for Windows but any .NET 6.0 or higher should do.
* [Download and install Visual Studio](https://visualstudio.microsoft.com/downloads/).
  I am using the 2022 Community edition, version 17.4.4 here but any 2019 or 2022 edition will be ok.
  Make sure to install the `Azure development` workload as we will be deploying with this later.
  If you already have an installed version of Visual Studio, go into the Installer and click `modify` to find it.
* [Sign up for a free Microsoft Azure account](https://azure.microsoft.com/en-us/free/).
* [Sign up for a free MongoDB Atlas account](https://account.mongodb.com/account/register).

## Creating a new Microsoft Blazor project that will contain our Crypto News Website

Now that the pre-requisites are out of the way, let's start by creating a new project.

![01_get_started.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/01_get_started_88f36b9147.jpg)

I have recently discovered [Microsoft Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) and I
absolutely love it.
Such an easy way to create websites quickly and easily. And you don't even have to write any JavaScript or PHP!
Let's use is for this tutorial as well. Search for `Blazor Server App` and click `Next`.

![02_create_a_new_project.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/02_create_a_new_project_53da789c4c.jpg)

Choose a `Project name` and `Location` of you liking.
I like to have the solution and project in the same directory but you don't have to.

![03_configure_your_new_project.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/03_configure_your_new_project_baec3276cd.jpg)

Choose your currently installed .NET framework (as described in `Pre-requisites`) and leave the rest on default.

![04_additional_information.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/04_additional_information_47949f55f7.jpg)

Hit `Create` and your good to go!

## Adding the MongoDB Driver to the project to connect to the database

Before we start getting into the code, we need to add one NuGet package to the project,
the [MongoDB Driver](https://www.mongodb.com/docs/drivers/).
The Driver is a library that let's you easily access
your [MongoDB Atlas cluster](https://www.mongodb.com/basics/clusters) and work with your database.
Click on `Project` -> `Manage NuGet Packages...` and search for `MongoDB.Driver`.

![07_manage_nuget_packages.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/07_manage_nuget_packages_664b480003.jpg)

During that process you might have to install additional components like the ones shown in the following screenshot.
Confirm this installation as we will need some of those as well.

![08_install_additional_components.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/08_install_additional_components_d8a428df2c.jpg)

Another message you come across might be the following license agreements which you need to accept to be able to work
with those libraries.

![09_accept_licenses.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/09_accept_licenses_21361afe2d.jpg)

## Creating a new MongoDB Atlas cluster and database to host our Crypto News

Now that we've installed the Driver, let's go ahead and create a cluster and database to connect to.

When you register a new account you will be presented with the selection of a cloud database to deploy.
For this tutorial we only need the forever-free shared tier. Choose Azure in the list of cloud providers and a
region of your liking.
Remember that choice so you can later on deploy your website to the same region on Azure.
The remaining options can be left on their defaults.

![12_deploy_a_cloud_database.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/12_deploy_a_cloud_database_b499ce0e74.jpg)
![13_create_a_shared_cluster.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/13_create_a_shared_cluster_8cdc5c9896.jpg)

The final step of creating a new cluster is to think about security measures by going through the `Security Quickstart`.

![30_security_quick_start.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/30_security_quick_start_d92b651359.jpg)

Choose a `Username` and `Password` for the database user that will access this cluster during the tutorial.
For the `Access List` we need add `0.0.0.0/0` since we do not know the IP address of our Azure deployment yet.
Now hit `Finish and Close`.

Creating a new shared cluster happens very, very fast and you should be able to start within minutes.
As soon as the cluster is created you'll see it in your list of `Database Deployments`.

Let's add some sample data for our website! Click on `Browse Collections` now.

![14_your_new_cluster](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/14_your_new_cluster_021c528d9d.jpg)

If you've never worked with Atlas before, here are some vocabularies to get your started:

- A cluster consists of multiple nodes (for redundancy).
- A cluster can contain multiple databases (which are replicated onto all nodes).
- Each database can contain many collections, which are similar to tables in a relational database.
- Each collection can then contain many documents. Think rows, just better!
  Documents are super flexible, easy to read and easy to work with JSON-like structures that contain our data.

## Creating some test data in Atlas

Since there is no data yet, you will see and empty list of databases and collections.
Click on `Add My Own Data` to add the first entry.

![15_collections.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/15_collections_601bf59508.jpg)

The database name and collection name can be anything but to be in line with the code we'll see later,
call them `crypto-news-website` and `news` respectively and hit `Create`.

![16_create_database.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/16_create_database_afa2bb61d2.jpg)

This should lead to a new entry that looks like this:

![17_your_new_collection.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/17_your_new_collection_25096bc341.jpg)

Next, click on `INSERT DOCUMENT`.

There are a couple things going on in the above. The `_id` has already been created automatically.
Each document contains one of those and they are of type `ObjectId`. It uniquely identifies the document.

By hovering over the line count on the left, you'll get a pop-op to add more fields.
Add one called `title` and set it's value to whatever you like, the above is an example you can use.
Choose `String` as the type on the right.
Next, add a `date` and choose `Date` as the type on the right.

![18_insert_document.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/18_insert_document_c7ce90362e.jpg)

Repeat the above process a couple times to get as much example data in there as you like.
You may also just continue with one entry though if you like.

## Connecting to Atlas via the Data API

The final step within MongoDB Atlas is to actually create access to this database so that the MongoDB Driver we
installed into the project can connect to it.
This is done by using a [connection string](https://www.mongodb.com/docs/manual/reference/connection-string/).
A connection string is a URI that contains username, password and the host address of the database you want
to connect to.

Click on `Databases` on the left to get back to the cluster overview.

This time, hit the `Connect` button and then `Connect Your Application`.
If you haven't done so before, choose a username and password the database user accessing this cluster during the
tutorial.
Also, add `0.0.0.0/0` as the IP address so that the Azure deployment can access the cluster later on.

Copy the `connection string` that's shown in the pop-up.

![19_connection_string.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/19_connection_string_cb44991831.jpg)

## Creating a new Blazor page

If you have never used Blazor before, just hit the `Run` button and have a look at the template that has been generated.
It's a great start and we will be re-using same parts of it later on.

Let's add our own page first though. In your Solution Explorer you'll see a `Pages` folder.
Right-click it and add a `Razor Component`. Those are files that combine the HTML of your page with C# code.

![05_add_a_new_component.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/05_add_a_new_component_6476440c47.jpg)
![06_name_the_new_component.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/06_name_the_new_component_8ecc33dea7.jpg)

Now, replace the content of the file with the following code. Explanations can be read inline in the code comments.

```csharp
// The `page` attribute defines how this page can be opened.
@page "/news"

// The `MongoDB` driver will be used to connect to your Atlas cluster.
@using MongoDB.Driver
// `BSON` is a file format similar to JSON. MongoDB Atlas documents are BSON documents.
@using MongoDB.Bson
// You need to add the `Data` folder as well. This is where the `News` class resides.
@using CryptoNewsApp.Data

// The page title is what your browser tab will be called. 
<PageTitle>News</PageTitle>

// Let's add a header to the page.
<h1>News</h1>

// And then some data.
// This is just a simple table contains news and their date.
@if (_news != null)
{
    <table class="table">
        <thead>
        <tr>
            <th>News</th>
            <th>Date</th>
        </tr>
        </thead>
        <tbody>
        // Blazor takes this data from the `_news` field that we will fill later on.
        @foreach (var newsEntry in _news)
        {
            <tr>
                <td>@newsEntry.Title</td>
                <td>@newsEntry.Date</td>
            </tr>
        }
        </tbody>
    </table>
}

// This part defines the code that will be run when the page is loaded. It's basically 
// what would usually be PHP in a non-Blazor environment.
@code {
    
    // The `_news` field will hold all our news. We will have a look at the `News`
    // class in just a moment.
    private List<News>? _news;

    // `OnInitializedAsync()` gets called when the website is loaded. Our data
    // retrieval logic has to be placed here.
    protected override async Task OnInitializedAsync()
    {
        // First, we need to create a `MongoClient` which is what we use to
        // connect to our cluster.
        // The only argument we need to pass on is the connection string you
        // retrieved from Atlas. Make sure to substitute the password with
        // the actual password.
        var mongoClient = new MongoClient("mongodb+srv://dbuser:dbUserPassword@cluster0.pl7egwa.mongodb.net/?retryWrites=true&w=majority");
        // Using the `mongoCLient` we can now access the database.
        var cryptoNewsDatabase = mongoClient.GetDatabase("crypto-news-database");
        // Having a handle to the database we can furthermore get the collection data.
        // Note that this is a generic function that takes `News` as it's paramter
        // to define who the documents in this collection look like.
        var newsCollection = cryptoNewsDatabase.GetCollection<News>("news");
        // Having access to the collection, we issue a `Find` call to find all documents.
        // A `Find` takes a filter as an argument. This filter is written as a `BsonDocument`.
        // Remember, `BSON` is really just a (binary) JSON.
        // Since we don't want to filter anything and get all the news, we pass along an
        // empty / new `BsonDocument`. The result is then transformed into a list with `ToListAsync()`.
        _news = await newsCollection.Find(new BsonDocument()).ToListAsync();
        // And that's it! It's as easy as that using the driver to access the data
        // in your MongoDB Atlas cluster.
    }

}
```

In the above, you've noticed the `News` class which still needs to be created.
In the `Data` folder, add a new C# class, call it `News` and use the following code.

```csharp
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CryptoNewsApp.Data
{
    public class News
    {
		// The attribute `BsonId` signals the MongoDB driver that this field 
		// should used to map the `_id` from the Atlas document.
		// Remember to use the type `ObjectId` here as well.
        [BsonId] public ObjectId Id { get; set; }

		// The two other fields in each news are `title` and `date`.
		// Since the C# coding style differs from the Atlas naming style, we have to map them.
		// Thankfully there is another handy attribute to achieve this: `BsonElement`.
		// It takes the document field's name and maps it to the classes field name.
        [BsonElement("title")] public String Title { get; set; }
        [BsonElement("date")] public DateTime Date { get; set; }
    }
}
```

Now it's time to look at the results. Hit `Run` again.
The website should open automatically, just add `/news` to the URL to see your new News page.

![20_news_website.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/20_news_website_4689c26e69.jpg)

## Deploying the website to Azure App Service

So far so good. Everything is running locally. Now to the fun part, going live!

And Visual Studio makes this super easy. Just click onto your project and choose `Publish...`.

![21_solution_explorer_publish.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/21_solution_explorer_publish_c9335f7064.jpg)

The target is `Azure`, the `Specific target` is `Azure App Service (Windows)`.

![22_publishing_target.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/22_publishing_target_f962875974.jpg)
![23_publishing_service.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/23_publishing_service_73e456e549.jpg)

When you registered for Azure earlier, a free subscription should have already been created and chosen here.
By clicking on `Create new` on the right you can now create a new App Service.

![24_create_new_app_service.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/24_create_new_app_service_a652f6f1c9.jpg)

The default settings are all totally fine. You can however choose a different region here if you want to.
Finally, click `Create` and then `Finish`.

![25_app_service_settings.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/25_app_service_settings_b1a05f9379.jpg)
![26_finish_service_creation.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/26_finish_service_creation_4b118a8f84.jpg)

When ready, the following pop-up should appear and by clicking `Publish` you can start the actual publishing process.
It eventually shows the result of the publish.

![27_publish_summary.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/27_publish_summary_fdc1933648.jpg)
![28_publish_succeeded.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/28_publish_succeeded_0b3b26a2b3.jpg)

The above summary will also show you the URL which was created for the deployment.
My example: https://cryptonewsapp20230124021236.azurewebsites.net/
Again, add `/news` to it to get to the News page.

![29_published_website.jpg](https://mongodb-devhub-cms.s3.us-west-1.amazonaws.com/29_published_website_eee29476a0.jpg)

## What's next?

Go ahead and add some more data. Add more fields or style the website a bit more than this default table.
The combination of using [Microsoft Azure](https://azure.microsoft.com/en-us/free/) and
[MongoDB Atlas](https://account.mongodb.com/account/register) makes it super easy and fast to create
websites like this one. But it is only the start.
You can learn more about Azure on the [Learn platform](https://learn.microsoft.com/en-us/training/) and
about Atlas on the [MongoDB University](https://learn.mongodb.com/).
And if you have any questions, please reach out to us at the
[MongoDB Forums](https://www.mongodb.com/community/forums/) or tweet [@dominicfrei](https://twitter.com/dominicfrei).

