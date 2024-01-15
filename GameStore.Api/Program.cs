using GameStore.Api.Entities;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGameResource";

List<Game> games =
    new()
    {
        new Game()
        {
            Id = 1,
            Name = "Street Fighter II",
            Genre = "Fighting",
            Price = 19.99M,
            ReleaseDate = new DateTime(1991, 2, 1),
            ImageUrl = "https://placehold.co/100"
        },
        new Game()
        {
            Id = 2,
            Name = "Final Fantasy XIV",
            Genre = "Role Playing",
            Price = 59.99M,
            ReleaseDate = new DateTime(2010, 9, 30),
            ImageUrl = "https://placehold.co/100"
        },
        new Game()
        {
            Id = 3,
            Name = "FIFA 23",
            Genre = "Sports",
            Price = 69.99M,
            ReleaseDate = new DateTime(2022, 9, 27),
            ImageUrl = "https://placehold.co/100"
        }
    };

app.MapGet(
    "/games",
    () =>
    {
        try
        {
            return Results.Ok(games);
        }
        catch (Exception ex)
        {
            return Results.Json(new { message = ex }, statusCode: 500);
        }
    }
);

app.MapGet(
        "/games/{id}",
        (int id) =>
        {
            try
            {
                Game? game = games.Find(g => g.Id == id);

                if (game is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok<Game>(game);
            }
            catch (Exception ex)
            {
                return Results.Json(new { message = ex }, statusCode: 500);
            }
        }
    )
    .WithName("GetGameResource");

app.MapPost(
    "/games",
    (Game game) =>
    {
        try
        {
            game.Id = games.Max(game => game.Id) + 1;
            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        }
        catch (Exception ex)
        {
            return Results.Json(new { message = ex }, statusCode: 500);
        }
    }
);

app.MapPut(
    "/games/{id}",
    (int id, Game UpdatedGame) =>
    {
        try
        {
            Game? existingGame = games.Find(g => g.Id == id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Name = UpdatedGame.Name;
            existingGame.Genre = UpdatedGame.Genre;
            existingGame.Price = UpdatedGame.Price;
            existingGame.ReleaseDate = UpdatedGame.ReleaseDate;
            existingGame.ImageUrl = UpdatedGame.ImageUrl;

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.Json(new { message = ex }, statusCode: 500);
        }
    }
);

app.MapDelete(
    "/games/{id}",
    (int id) =>
    {
        try
        {
            Game? game = games.Find(g => g.Id == id);

            if (game is not null)
            {
                games.Remove(game);
            }

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.Json(new { message = ex }, statusCode: 500);
        }
    }
);

app.Run();
