[![Banner Page.jpg](https://s9.postimg.org/7hdnvdgdr/Banner_Page.jpg)](https://postimg.org/image/fmvptj4mj/)

Harmonics is an MVC service web application with database read and write functionality, user purchase logging using Entity Framework, and customized Spotify Widget injection. The scope of the application was a music store management system where employees could create new customer records and assign purchases to any given customer either at the time of creation or later. This app was used to solve common MVC problems inlcuding displaying unique data gathered from a database into various views using Razor syntax, and assigning customer purchases while a customer is first created.

## Libraries and Resources Used 

- [Contoso University](https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-an-entity-framework-data-model-for-an-asp-net-mvc-application) - A great MVC 5 and Entity Framework sample project / walkthrough
- [Mosh Hamedani - Become a .NET Full Stack Developer](https://www.pluralsight.com/courses/full-stack-dot-net-developer-fundamentals) - Awesome pluralsite .NET course


## How to run locally

1. Clone the repo 

2. Open the project and set up Home/Index as the start up file in not already

3. Start running the application. 

4. Select Go To Customers where you can create a new customer and assign purchased albums to their records. 

6. View Album Details to see the customer spotify widget and album art associated to each album

## Index

* [Assigning Purchases to New Customers](#customer)
* [Spotify and AlbumArt Injection](#injection)

---

## Customer



Since an employee needs the ability to assign customer purchases at the time of customer creation. We need to have the database query all the albums available for purchase and display them as checkable items in the customer create view. We then need those purchased albums to save to the database.

1. First, we need to add a Populate albums method in the GET ActionResult Create()
    
```cs
        public ActionResult Create()
        {
            var customer = new Customer();
            customer.Purchases = new List<Purchases>();
            PopulatePurchasedAlbums(customer);

            return View();
        }
```

2. Once the create GET is called for the new customer here is the PopulatePurchasedAlbums method that will execute with it. This method looks at the the db.Albums column and stores all available albums into the ViewBag.Albums variable.


```cs

        private void PopulatePurchasedAlbums(Customer customer)
        {
            var albums = db.Albums;
            var customerPurchases = new HashSet<int>(customer.Purchases.Select(c => c.AlbumID));
            var viewModel = new List<AlbumViewModel>();
            foreach (var album in albums)
            {
                viewModel.Add(new AlbumViewModel
                {
                    AlbumID = album.AlbumID,
                    Title = album.Title,
                    Artist = album.Artist,
                    Assigned = customerPurchases.Contains(album.AlbumID)
                });
            }
            ViewBag.Albums = viewModel;
        }


   
```

3. We then take the ViewBag.Albums variable and use it to create a list of checkboxes of all albums from the db set inside the create View. This is also storing the AlbumID of each checked checkbox (customer purchase) in the "purchasedAlbums" value. 

```cs
         <table class="table-responsive col-lg-pull-2">
            <tr>
                @{                    
                    List<MegaStoreApp.ViewModels.AlbumViewModel> albums = ViewBag.Albums;

                    foreach (var album in albums)
                    {
                        
                        @:</tr><tr>
                        @:<td>
                            <input type="checkbox"
                                name="purchasedAlbums"
                                value="@album.AlbumID"/>
                                @album.Artist@: - @album.Title
                        @:</td>
                    }
                    @:</tr>
                }
        </table>
```

The form at this point will look similar to this

[![NewCustomer.jpg](https://s9.postimg.org/8iumpuve7/New_Customer.jpg)](https://postimg.org/image/5c0368ay3/)

4. With all the checked albumIDs stored in "purchaseAlbums" we take it back to the controller and pass the variable into the Create ActionResult. This action result first saves the customer then parses the purchasedAlbums array back into individual albums and binds those them to the customer.
```cs
       [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,LastName,FirstMidName,CreationDate")] Customer customer, string[] purchasedAlbums)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
            if (purchasedAlbums != null)
            {
                                
                foreach (var purchase in purchasedAlbums)
                {                    
                    customer.AlbumID = Int32.Parse(purchase);
                    
                    Purchases p = new Purchases
                    {
                        CustomerID = customer.CustomerID,
                        AlbumID = customer.AlbumID,
                        
                    };                   
                    db.Purchases.Add(p);                    
                }
            }


            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            db.SaveChanges();            
            return View(customer);
        }
```

## Injection 


Although the main problem of the app had been solved, I was interested in experimenting with how to inject unique artwork and spotify url's for each album.

1.The album records are created in the StoreInitializer file. See how when initialized both a content path to the Album Art, and a sample spotify song url are saved to the database. 

```cs
            var albums = new List<Album>
            {
            new Album{AlbumID=1050,Artist = "Kendrick Lamar",Title="Damn",Genre="Hip-Hop",Price=9.99M,
                AlbumArtLocation ="~/Content/Images/kendrick.jpg",
                SpotifyURL ="https://embed.spotify.com/?uri=spotify%3Atrack%3A6HZILIRieu8S0iqY8kIKhj"},
            new Album{AlbumID=4022,Artist = "Daft Punk", Title="RAM",Genre="Electronic",Price=9.99M,
                AlbumArtLocation ="~/Content/Images/daftpunk.jpg",
                SpotifyURL ="https://embed.spotify.com/?uri=spotify%3Atrack%3A36c4JohayB9qd64eidQMBi"},
            new Album{AlbumID=4041,Artist = "Frankie Cosmos", Title="Next Thing",Genre="Indie Rock",Price=7.99M,
                AlbumArtLocation ="~/Content/Images/frankiecosmos.jpg",
                SpotifyURL ="https://embed.spotify.com/?uri=spotify%3Atrack%3A5MAbv1SXZX03D4ndmn1CEZ"},
            new Album{AlbumID=1045,Artist = "Frank Ocean", Title="Blonde",Genre="Hip-Hop",Price=9.99M,
                AlbumArtLocation ="~/Content/Images/frankocean.jpg",
                SpotifyURL ="https://embed.spotify.com/?uri=spotify%3Atrack%3A6Nle9hKrkL1wQpwNfEkxjh"},
            new Album{AlbumID=3141,Artist = "The Knife", Title="Silent Shout", Genre = "Electronic",Price=5.57M,
                AlbumArtLocation ="~/Content/Images/theknife.jpg",
                SpotifyURL ="https://embed.spotify.com/?uri=spotify%3Atrack%3A0ONbiyqCsjVSjrrZJISZIY"}
            };
            albums.ForEach(s => context.Albums.Add(s));
            context.SaveChanges();
```

2. We then can use that information on the album details View like this

```cs

    @model MegaStoreApp.Models.Album

    <img src="@Url.Content(Model.AlbumArtLocation)" class="img-thumbnail" width="304" height="236" />

    <div class="media dl-horizontal col-md-offset-7">

        <iframe src="@Model.SpotifyURL"
                width="300" height="80" frameborder="0" allowtransparency="true"></iframe>

    </div>
```

3. Since this View is generated for each album id individual this allows you to insert this code once, and it will render the unqiue information stored for each album and change both the image and spotify widget. The final result should look something like this

[![Album Details.jpg](https://s12.postimg.org/kt92bkutp/Album_Details.jpg)](https://postimg.org/image/ibxb4bax5/)

## Conclusion

I hope you found this helpful, you can email me with any questions at ben.micah.peterson@gmail.com