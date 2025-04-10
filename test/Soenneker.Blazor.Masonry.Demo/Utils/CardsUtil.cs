using System;
using System.Collections.Generic;
using Soenneker.Blazor.Masonry.Demo.Dtos;
using Soenneker.Utils.AutoBogus;
using Soenneker.Utils.Random;

namespace Soenneker.Blazor.Masonry.Demo.Utils;

public static class CardsUtil
{
    private static readonly AutoFaker _autoFaker = new();

    public static List<CardModel> GetCards()
    {
        var cards = new List<CardModel>();

        for (var i = 1; i <= 12; i++)
        {
            int height = RandomUtil.Next(80, 320);
            var imageUrl = $"https://picsum.photos/300/{height}?random={Guid.NewGuid().ToString()}";

            string text;
            switch (i % 3)
            {
                case 0:
                    text = _autoFaker.Faker.Lorem.Sentence();
                    break;
                case 1:
                    text = "Quick summary.";
                    break;
                default:
                    text = "This is a mid-sized description with a little more detail to show variation.";
                    break;
            }

            cards.Add(new CardModel
            {
                Title = $"Card Title {i}",
                ImageUrl = imageUrl,
                Text = text
            });
        }

        return cards;
    }
}