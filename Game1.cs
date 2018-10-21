using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Solitaire
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        bool stopTimer = true;
        int hour, minute, seconds;
        bool smat = false;
        bool firstTime = true;
        Boolean showCursor = false;
        Texture2D cursorTex;
        Texture2D[] bg = new Texture2D[137];
        Texture2D[] bgg = new Texture2D[24];
        Texture2D pot;
        Boolean clickNext = false;
        Boolean s_matCard = false;
        Boolean s_potCard = false;
        Point currentMousePosition;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        List<Card> mystack;
        List<Card> cards;
        List<Card> cardsMat;
        List<Card> stack1;
        List<Card> stack2;
        List<Card> stack3;
        List<Card> stack4;
        List<Card> stack5;
        List<Card> stack6;
        List<Card> stack7;
        List<Card> stack;
        Texture2D card;
        Texture2D back;
        Texture2D beginPlayT;

        MouseState _currentMouseState;
        MouseState _previousMouseState;

        int currentCard;
        int curentCard;

        bool gameOver = true;
        bool againCards = true;
        bool beginPlayClicked;
        bool selectedMatCard = false;
        bool selectedPotCard = false;
        bool selectedStackCard = false;
        bool selectedAnywhere = false;
        int iibgg = 0;
        int ii = 0;
        int cntt = 0;
        int cntt2 = 0;

        int countStack1 = 0, countStack2 = 0, countStack3 = 0, countStack4 = 0;
        int insertStack1 = -1, insertStack2 = -1, insertStack3 = -1, insertStack4 = -1;
        string whichstack = "stack1";
        int selectedPast = -1;
        int selectedS = -1;
        int selectedStack = -1;
        int selectedStackIndex = -1;
        int selectedPaste = -1;
        int selectedPIndex = -1;
        int selectedFourIndex = -1;
        int selectedPasteIndex = -1;
        bool continuePot = false;
        bool hoverPlay = false;
        string stakHover = "";
        bool stkChanged = false;
        bool mystackclear = false;

        private void st_timer()
        {
            while (true)
            {
                if (!stopTimer)
                {
                    new System.Threading.ManualResetEvent(false).WaitOne(1000);
                    if(seconds < 60)
                        seconds++;
                    if(seconds == 60)
                    {
                        seconds = 0;
                        minute++;
                    }
                    if (minute == 60)
                    {
                        minute = 0;
                        hour++;
                    }
                }
            }
        }

        public Game1()
            : base()
        {
            this.Window.Title = "Solitaire Decked Out";

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 700;

            this.graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            SpriteBatchEx.GraphicsDevice = GraphicsDevice;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            for (int i = 0; i < 24; i++)
            {
                string aa = "" + i;
                if (aa.Length == 1)
                    aa = "0" + aa;
                bgg[i] = Content.Load<Texture2D>("frame_" + aa + "_delay-0.08s");
            }

            for (int i = 0; i < 137; i++)
            {
                string aaa = "" + i;
                if (aaa.Length == 1)
                    aaa = "00" + aaa;
                else if (aaa.Length == 2)
                    aaa = "0" + aaa;
                try
                {
                    bg[i] = Content.Load<Texture2D>("frame_" + aaa + "_delay-0.06s");
                }
                catch(Exception e1)
                {
                    try
                    {
                        string str = e1.Message;
                        bg[i] = Content.Load<Texture2D>("frame_" + aaa + "_delay-0.07s");
                    }
                    catch (Exception e2)
                    {
                        string str = e2.Message;
                        bg[i] = Content.Load<Texture2D>("frame_" + aaa + "_delay-s");
                    }
                }
            }
            pot = Content.Load<Texture2D>("pot");
            cursorTex = Content.Load<Texture2D>("mouseHand");
            spriteFont = Content.Load<SpriteFont>("SpriteFont1");
            back = Content.Load<Texture2D>("back");
            beginPlayT = Content.Load<Texture2D>("playBtn");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void goNextMatCard()
        {
            if (cardsMat.Count == 0 || clickNext)
            {
                if (cardsMat.Count > 0)
                    clickNext = false;
                if (cards.Count == 0)
                {
                    currentCard = 0;
                    if (cardsMat.Count > 0)
                    {
                        for (int i = 0; i < cardsMat.Count; i++)
                            cards.Add(cardsMat[i]);
                        cardsMat.Clear();
                    }
                }
                try
                {
                    s_matCard = false;
                    s_potCard = false;
                    cards[0].whatplace = "mat";
                    cardsMat.Add(cards[0]);
                    cards.Remove(cards[0]);
                    currentCard++;
                }
                catch (Exception e)
                {
                    String str = e.Message;
                }
            }
            beginPlayClicked = false;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            if(firstTime)
            {
                gameOver = false;

                cards = new List<Card>();

                for (int suit = Card.DIAMOND; suit <= Card.SPADE; suit++)
                {
                    for (int rank = 1; rank <= 13; rank++)
                    {
                        Card card = new Card();
                        cards.Add(card);

                        card.rank = rank;
                        card.suit = suit;
                        card.place = new Rectangle(160+5, 20, 80, 160);

                        if (rank == 1)
                        {
                            card.card = "ace";
                        }
                        else if (rank == 2)
                        {
                            card.card = "two";
                        }
                        else if (rank == 3)
                        {
                            card.card = "three";
                        }
                        else if (rank == 4)
                        {
                            card.card = "four";
                        }
                        else if (rank == 5)
                        {
                            card.card = "five";
                        }
                        else if (rank == 6)
                        {
                            card.card = "six";
                        }
                        else if (rank == 7)
                        {
                            card.card = "seven";
                        }
                        else if (rank == 8)
                        {
                            card.card = "eight";
                        }
                        else if (rank == 9)
                        {
                            card.card = "nine";
                        }
                        else if (rank == 10)
                        {
                            card.card = "ten";
                        }
                        else if (rank == 11)
                        {
                            card.card = "jack";
                        }
                        else if (rank == 12)
                        {
                            card.card = "queen";
                        }
                        else if (rank == 13)
                        {
                            card.card = "king";
                        }
                    }
                }

                do
                {
                    cards = FisherYates.Shuffle(cards);
                } while (cards[0].rank == 1);

                cardsMat = new List<Card>();
                mystack = new List<Card>();
                stack1 = new List<Card>();
                stack2 = new List<Card>();
                stack3 = new List<Card>();
                stack4 = new List<Card>();
                stack5 = new List<Card>();
                stack6 = new List<Card>();
                stack7 = new List<Card>();
                stack = new List<Card>();

                s_matCard = false;
                s_potCard = false;
                dealCards();
                againCards = false;
                beginPlayClicked = false;

                System.Threading.Thread t = new System.Threading.Thread(st_timer);
                t.Start();

                stopTimer = false;

                hour = 0;
                minute = 0;
                seconds = 0;

                firstTime = false;
            }

            if (close())
            {
                Application.Exit();
            }

            if (beginPlay())
            {
                if (beginPlayClicked || againCards) {
                    s_matCard = false;
                    s_potCard = false;
                    dealCards();
                    againCards = false;
                    beginPlayClicked = false;
                    mystack.Clear();
                    selectedPIndex = -1;
                    selectedStackIndex = -1;
                    selectedStack = -1;
                    stopTimer = false;
                    hour = 0;
                    minute = 0;
                    seconds = 0;
                }
            }

            if(nexts())
            {
                if (beginPlayClicked) {
                    goNextMatCard();
                }
            }

            selectAnywhere();

            if (selectMatCard())
            {
                selectedMatCard = true;
                s_matCard = true;
                selectedFourIndex = -1;
            }

            try
            {
                if (selectedPasteIndex != -1 && (selectedPaste == selectedPast || selectedPaste == 7 || selectedPaste == 6) && selectedPast != -1 && selectedPaste != -1)
                {
                    if (selectedPaste == 1 && selectedPasteIndex == stack1.Count - 1)
                    {
                        if (cardsMat[cardsMat.Count - 1].rank == stack1[stack1.Count - 1].rank - 1)
                        {
                            if (cardsMat[cardsMat.Count - 1].suit == Card.CLUB || cardsMat[cardsMat.Count - 1].suit == Card.SPADE)
                            {
                                if (stack1[stack1.Count - 1].suit == Card.HEART || stack1[stack1.Count - 1].suit == Card.DIAMOND)
                                {
                                    stack1.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack1[stack1.Count - 1].back = false;
                                    stack1[stack1.Count - 1].whatplace = "stak1";
                                    goNextMatCard();
                                    selectedMatCard = false;
                                    s_matCard = false;
                                }
                            }
                            else if (cardsMat[cardsMat.Count - 1].suit == Card.HEART || cardsMat[cardsMat.Count - 1].suit == Card.DIAMOND)
                            {
                                if (stack1[stack1.Count - 1].suit == Card.CLUB || stack1[stack1.Count - 1].suit == Card.SPADE)
                                {
                                    stack1.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack1[stack1.Count - 1].back = false;
                                    stack1[stack1.Count - 1].whatplace = "stak1";
                                    goNextMatCard();
                                    selectedMatCard = false;
                                    s_matCard = false;
                                }
                            }
                        }
                    }
                    if (selectedPaste == 2 && selectedPasteIndex == stack2.Count - 1)
                    {
                        if (cardsMat[cardsMat.Count - 1].rank == stack2[stack2.Count - 1].rank - 1)
                        {
                            if (cardsMat[cardsMat.Count - 1].suit == Card.CLUB || cardsMat[cardsMat.Count - 1].suit == Card.SPADE)
                            {
                                if (stack2[stack2.Count - 1].suit == Card.HEART || stack2[stack2.Count - 1].suit == Card.DIAMOND)
                                {
                                    stack2.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack2[stack2.Count - 1].back = false;
                                    stack2[stack2.Count - 1].whatplace = "stak2";
                                    goNextMatCard();
                                    selectedMatCard = false;
                                    s_matCard = false;
                                }
                            }
                            else if (cardsMat[cardsMat.Count - 1].suit == Card.HEART || cardsMat[cardsMat.Count - 1].suit == Card.DIAMOND)
                            {
                                if (stack2[stack2.Count - 1].suit == Card.CLUB || stack2[stack2.Count - 1].suit == Card.SPADE)
                                {
                                    stack2.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack2[stack2.Count - 1].back = false;
                                    stack2[stack2.Count - 1].whatplace = "stak2";
                                    goNextMatCard();
                                    selectedMatCard = false;
                                    s_matCard = false;
                                }
                            }
                        }
                    }
                    if (selectedPaste == 3 && selectedPasteIndex == stack3.Count - 1)
                    {
                        if (cardsMat[cardsMat.Count - 1].rank == stack3[stack3.Count - 1].rank - 1)
                        {
                            if (cardsMat[cardsMat.Count - 1].suit == Card.CLUB || cardsMat[cardsMat.Count - 1].suit == Card.SPADE)
                            {
                                if (stack3[stack3.Count - 1].suit == Card.HEART || stack3[stack3.Count - 1].suit == Card.DIAMOND)
                                {
                                    stack3.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack3[stack3.Count - 1].back = false;
                                    stack3[stack3.Count - 1].whatplace = "stak3";
                                    goNextMatCard();
                                    selectedMatCard = false;
                                    s_matCard = false;
                                }
                            }
                            else if (cardsMat[cardsMat.Count - 1].suit == Card.HEART || cardsMat[cardsMat.Count - 1].suit == Card.DIAMOND)
                            {
                                if (stack3[stack3.Count - 1].suit == Card.CLUB || stack3[stack3.Count - 1].suit == Card.SPADE)
                                {
                                    stack3.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack3[stack3.Count - 1].back = false;
                                    stack3[stack3.Count - 1].whatplace = "stak3";
                                    goNextMatCard();
                                    selectedMatCard = false;
                                    s_matCard = false;
                                }
                            }
                        }
                    }
                    if (selectedPaste == 4 && selectedPasteIndex == stack4.Count - 1)
                    {
                        if (cardsMat[cardsMat.Count - 1].rank == stack4[stack4.Count - 1].rank - 1)
                        {
                            if (cardsMat[cardsMat.Count - 1].suit == Card.CLUB || cardsMat[cardsMat.Count - 1].suit == Card.SPADE)
                            {
                                if (stack4[stack4.Count - 1].suit == Card.HEART || stack4[stack4.Count - 1].suit == Card.DIAMOND)
                                {
                                    stack4.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack4[stack4.Count - 1].back = false;
                                    stack4[stack4.Count - 1].whatplace = "stak4";
                                    goNextMatCard();
                                    selectedMatCard = false;
                                    s_matCard = false;
                                }
                            }
                            else if (cardsMat[cardsMat.Count - 1].suit == Card.HEART || cardsMat[cardsMat.Count - 1].suit == Card.DIAMOND)
                            {
                                if (stack4[stack4.Count - 1].suit == Card.CLUB || stack4[stack4.Count - 1].suit == Card.SPADE)
                                {
                                    stack4.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack4[stack4.Count - 1].back = false;
                                    stack4[stack4.Count - 1].whatplace = "stak4";
                                    goNextMatCard();
                                    selectedMatCard = false;
                                    s_matCard = false;
                                }
                            }
                        }
                    }
                    if (selectedPaste == 5 && selectedPasteIndex == stack5.Count - 1)
                    {
                        if (cardsMat[cardsMat.Count - 1].rank == stack5[stack5.Count - 1].rank - 1)
                        {
                            if (cardsMat[cardsMat.Count - 1].suit == Card.CLUB || cardsMat[cardsMat.Count - 1].suit == Card.SPADE)
                            {
                                if (stack5[stack5.Count - 1].suit == Card.HEART || stack5[stack5.Count - 1].suit == Card.DIAMOND)
                                {
                                    stack5.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack5[stack5.Count - 1].back = false;
                                    stack5[stack5.Count - 1].whatplace = "stak5";
                                    goNextMatCard();
                                    selectedMatCard = false;
                                    s_matCard = false;
                                }
                            }
                            else if (cardsMat[cardsMat.Count - 1].suit == Card.HEART || cardsMat[cardsMat.Count - 1].suit == Card.DIAMOND)
                            {
                                if (stack5[stack5.Count - 1].suit == Card.CLUB || stack5[stack5.Count - 1].suit == Card.SPADE)
                                {
                                    stack5.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack5[stack5.Count - 1].back = false;
                                    stack5[stack5.Count - 1].whatplace = "stak5";
                                    goNextMatCard();
                                    selectedMatCard = false;
                                    s_matCard = false;
                                }
                            }
                        }
                    }
                    if (selectedPaste == 6 && selectedPasteIndex == stack6.Count - 1)
                    {
                        if (cardsMat[cardsMat.Count - 1].rank == stack6[stack6.Count - 1].rank - 1)
                        {
                            if (cardsMat[cardsMat.Count - 1].suit == Card.CLUB || cardsMat[cardsMat.Count - 1].suit == Card.SPADE)
                            {
                                if (stack6[stack6.Count - 1].suit == Card.HEART || stack6[stack6.Count - 1].suit == Card.DIAMOND)
                                {
                                    stack6.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack6[stack6.Count - 1].back = false;
                                    stack6[stack6.Count - 1].whatplace = "stak6";
                                    goNextMatCard();
                                    selectedMatCard = false;
                                    s_matCard = false;
                                }
                            }
                            else if (cardsMat[cardsMat.Count - 1].suit == Card.HEART || cardsMat[cardsMat.Count - 1].suit == Card.DIAMOND)
                            {
                                if (stack6[stack6.Count - 1].suit == Card.CLUB || stack6[stack6.Count - 1].suit == Card.SPADE)
                                {
                                    stack6.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack6[stack6.Count - 1].back = false;
                                    stack6[stack6.Count - 1].whatplace = "stak6";
                                    goNextMatCard();
                                    selectedMatCard = false;
                                    s_matCard = false;
                                }
                            }
                        }
                    }
                    if (selectedPaste == 7 && selectedPasteIndex == stack7.Count - 1)
                    {
                        if (cardsMat[cardsMat.Count - 1].rank == stack7[stack7.Count - 1].rank - 1)
                        {
                            if (cardsMat[cardsMat.Count - 1].suit == Card.CLUB || cardsMat[cardsMat.Count - 1].suit == Card.SPADE)
                            {
                                if (stack7[stack7.Count - 1].suit == Card.HEART || stack7[stack7.Count - 1].suit == Card.DIAMOND)
                                {
                                    stack7.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack7[stack7.Count - 1].back = false;
                                    stack7[stack7.Count - 1].whatplace = "stak7";
                                    goNextMatCard();
                                    selectedMatCard = false;
                                    s_matCard = false;
                                }
                            }
                            else if (cardsMat[cardsMat.Count - 1].suit == Card.HEART || cardsMat[cardsMat.Count - 1].suit == Card.DIAMOND)
                            {
                                if (stack7[stack7.Count - 1].suit == Card.CLUB || stack7[stack7.Count - 1].suit == Card.SPADE)
                                {
                                    stack7.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack7[stack7.Count - 1].back = false;
                                    stack7[stack7.Count - 1].whatplace = "stak7";
                                    goNextMatCard();
                                    selectedMatCard = false;
                                    s_matCard = false;
                                }
                            }
                        }
                    }
                }
                else if ((selectedPast == 1 && stack1.Count == 0) ||
                    (selectedPast == 2 && stack2.Count == 0) ||
                    (selectedPast == 3 && stack3.Count == 0) ||
                    (selectedPast == 4 && stack4.Count == 0) ||
                    (selectedPast == 5 && stack5.Count == 0) ||
                    (selectedPast == 6 && stack6.Count == 0) ||
                    (selectedPast == 7 && stack7.Count == 0))
                {
                    if (selectedPast == 1 && selectedPasteIndex == -1)
                    {
                        if (cardsMat[cardsMat.Count - 1].rank == 13 ||
                            cardsMat[cardsMat.Count - 1].rank == 1)
                        {
                            stack1.Add(cardsMat[cardsMat.Count - 1]);
                            cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                            stack1[stack1.Count - 1].back = false;
                            stack1[stack1.Count - 1].whatplace = "stak1";
                            goNextMatCard();
                            selectedMatCard = false;
                            s_matCard = false;
                        }
                    }
                    if (selectedPast == 2 && selectedPasteIndex == -1)
                    {
                        if (cardsMat[cardsMat.Count - 1].rank == 13 ||
                            cardsMat[cardsMat.Count - 1].rank == 1)
                        {
                            stack2.Add(cardsMat[cardsMat.Count - 1]);
                            cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                            stack2[stack2.Count - 1].back = false;
                            stack2[stack2.Count - 1].whatplace = "stak2";
                            goNextMatCard();
                            selectedMatCard = false;
                            s_matCard = false;
                        }
                    }
                    if (selectedPast == 3 && selectedPasteIndex == -1)
                    {
                        if (cardsMat[cardsMat.Count - 1].rank == 13 ||
                            cardsMat[cardsMat.Count - 1].rank == 1)
                        {
                            stack3.Add(cardsMat[cardsMat.Count - 1]);
                            cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                            stack3[stack3.Count - 1].back = false;
                            stack3[stack3.Count - 1].whatplace = "stak3";
                            goNextMatCard();
                            selectedMatCard = false;
                            s_matCard = false;
                        }
                    }
                    if (selectedPast == 4 && selectedPasteIndex == -1)
                    {
                        if (cardsMat[cardsMat.Count - 1].rank == 13 ||
                            cardsMat[cardsMat.Count - 1].rank == 1)
                        {
                            stack4.Add(cardsMat[cardsMat.Count - 1]);
                            cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                            stack4[stack4.Count - 1].back = false;
                            stack4[stack4.Count - 1].whatplace = "stak4";
                            goNextMatCard();
                            selectedMatCard = false;
                            s_matCard = false;
                        }
                    }
                    if (selectedPast == 5 && selectedPasteIndex == -1)
                    {
                        if (cardsMat[cardsMat.Count - 1].rank == 13 ||
                            cardsMat[cardsMat.Count - 1].rank == 1)
                        {
                            stack5.Add(cardsMat[cardsMat.Count - 1]);
                            cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                            stack5[stack5.Count - 1].back = false;
                            stack5[stack5.Count - 1].whatplace = "stak5";
                            goNextMatCard();
                            selectedMatCard = false;
                            s_matCard = false;
                        }
                    }
                    if (selectedPast == 6 && selectedPasteIndex == -1)
                    {
                        if (cardsMat[cardsMat.Count - 1].rank == 13 ||
                            cardsMat[cardsMat.Count - 1].rank == 1)
                        {
                            stack6.Add(cardsMat[cardsMat.Count - 1]);
                            cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                            stack6[stack6.Count - 1].back = false;
                            stack6[stack6.Count - 1].whatplace = "stak6";
                            goNextMatCard();
                            selectedMatCard = false;
                            s_matCard = false;
                        }
                    }
                    if (selectedPast == 7 && selectedPasteIndex == -1)
                    {
                        if (cardsMat[cardsMat.Count - 1].rank == 13 ||
                            cardsMat[cardsMat.Count - 1].rank == 1)
                        {
                            stack7.Add(cardsMat[cardsMat.Count - 1]);
                            cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                            stack7[stack7.Count - 1].back = false;
                            stack7[stack7.Count - 1].whatplace = "stak7";
                            goNextMatCard();
                            selectedMatCard = false;
                            s_matCard = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                String str = e.Message;
            }

            if (stackClick())
            {
                int countStack = 0;
                for (int i = 0; i < stack.Count; i++)
                {
                    Card card = stack[i];
                    if (card.whatplace.Equals(whichstack))
                    {
                        countStack++;
                    }
                }
                if (curentCard != cardsMat.Count - 1 && curentCard > cardsMat.Count - 1)
                {
                    curentCard = curentCard - 24;
                }
                if(curentCard < 0)
                {
                    if (cardsMat.Count == 0)
                        curentCard = 0;
                    else if (cardsMat.Count > 0)
                        curentCard = cardsMat.Count - 1;
                }
                if (cardsMat.Count > 0 && smat)
                {
                    try
                    {
                        if (countStack == 0 && cardsMat[cardsMat.Count - 1].card.Equals("ace"))
                        {
                            Card newCard = new Card();
                            newCard.card = "ace";
                            newCard.rank = cardsMat[cardsMat.Count - 1].rank;
                            newCard.suit = cardsMat[cardsMat.Count - 1].suit;
                            newCard.whatplace = whichstack;
                            stack.Add(newCard);
                            cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                            s_potCard = true;
                            selectedStack = -2;
                            s_matCard = false;
                        }
                        else
                        {
                            int nets = -1;
                            for (int i = 0; i < stack.Count; i++)
                            {
                                Card card = stack[i];
                                if (card.whatplace.Equals(whichstack))
                                {
                                    if (cardsMat[cardsMat.Count - 1].rank - 1 == card.rank && card.suit.Equals(cardsMat[cardsMat.Count - 1].suit))
                                        nets = i + 1;
                                }
                            }
                            if (nets != -1)
                            {
                                cardsMat[cardsMat.Count - 1].whatplace = whichstack;
                                stack.Insert(nets, cardsMat[cardsMat.Count - 1]);
                                cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                s_potCard = true;
                                selectedStack = -2;
                                s_matCard = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        String str = e.Message;
                    }
                }
            }

            stakHoverTest();

            mystackclear = false;

            if (selectPotCard())
            {
                selectedMatCard = false;
                selectedStackCard = false;
                selectedPotCard = true;
                s_potCard = true;
                s_matCard = false;
            }

            if (selectedMatCard)
            {
                selectedStackCard = false;
                s_potCard = false;
            }

            if (selectedPotCard)
                s_matCard = false;

            if (selectStackCard())
            {
                selectedStackCard = true;
                selectedMatCard = false;
            }

            Boolean sss = stackClick();

            if (selectedStackCard)
            {
                if (sss)
                {
                    selectedStack = selectedS;

                    int countStack = 0;
                    int stackIndex = -1;

                    if (whichstack.Equals("stack1"))
                    {
                        for (int i = 0; i < stack.Count; i++)
                        {
                            if (stack[i].whatplace.Equals("stack1"))
                            {
                                countStack++;
                                stackIndex = i;
                            }
                        }
                    }
                    if (whichstack.Equals("stack2"))
                    {
                        for (int i = 0; i < stack.Count; i++)
                        {
                            if (stack[i].whatplace.Equals("stack2"))
                            {
                                countStack++;
                                stackIndex = i;
                            }
                        }
                    }
                    if (whichstack.Equals("stack3"))
                    {
                        for (int i = 0; i < stack.Count; i++)
                        {
                            if (stack[i].whatplace.Equals("stack3"))
                            {
                                countStack++;
                                stackIndex = i;
                            }
                        }
                    }
                    if (whichstack.Equals("stack4"))
                    {
                        for (int i = 0; i < stack.Count; i++)
                        {
                            if (stack[i].whatplace.Equals("stack4"))
                            {
                                countStack++;
                                stackIndex = i;
                            }
                        }
                    }
                    int compareTo = -1, compareTo2 = -1;
                    if (selectedStack == 1)
                    {
                        if (mystack.Count > 0)
                        {
                            if (countStack != 0)
                            {
                                compareTo = stack[stackIndex].rank + 1;
                                compareTo2 = stack[stackIndex].suit;
                            }
                            else
                            {
                                compareTo = 1;
                                compareTo2 = mystack[mystack.Count - 1].suit;
                            }
                            if (mystack[mystack.Count - 1].rank == compareTo &&
                                mystack[mystack.Count - 1].suit == compareTo2)
                            {
                                s_matCard = false;
                                selectedPIndex = -1;
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = whichstack;
                                newCard.rank = compareTo;
                                newCard.suit = compareTo2;
                                int index = 0;
                                if (stackIndex == -1)
                                    index = 0;
                                else
                                    index = stackIndex + 1;
                                stack.Insert(index, newCard);
                                mystack.Clear();
                                if (stack1.Count != 0)
                                    stack1[stack1.Count - 1].back = false;
                            }
                        }
                    }
                    if (selectedStack == 2)
                    {
                        if (mystack.Count > 0)
                        {
                            if (countStack != 0)
                            {
                                compareTo = stack[stackIndex].rank + 1;
                                compareTo2 = stack[stackIndex].suit;
                            }
                            else
                            {
                                compareTo = 1;
                                compareTo2 = mystack[mystack.Count - 1].suit;
                            }
                            if (mystack[mystack.Count - 1].rank == compareTo &&
                                mystack[mystack.Count - 1].suit == compareTo2)
                            {
                                s_matCard = false;
                                selectedPIndex = -1;
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = whichstack;
                                newCard.rank = compareTo;
                                newCard.suit = compareTo2;
                                int index = 0;
                                if (stackIndex == -1)
                                    index = 0;
                                else
                                    index = stackIndex + 1;
                                stack.Insert(index, newCard);
                                mystack.Clear();
                                if (stack2.Count != 0)
                                    stack2[stack2.Count - 1].back = false;
                            }
                        }
                    }
                    if (selectedStack == 3)
                    {
                        if (mystack.Count > 0)
                        {
                            if (countStack != 0)
                            {
                                compareTo = stack[stackIndex].rank + 1;
                                compareTo2 = stack[stackIndex].suit;
                            }
                            else
                            {
                                compareTo = 1;
                                compareTo2 = mystack[mystack.Count - 1].suit;
                            }
                            if (mystack[mystack.Count - 1].rank == compareTo &&
                                mystack[mystack.Count - 1].suit == compareTo2)
                            {
                                s_matCard = false;
                                selectedPIndex = -1;
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = whichstack;
                                newCard.rank = compareTo;
                                newCard.suit = compareTo2;
                                int index = 0;
                                if (stackIndex == -1)
                                    index = 0;
                                else
                                    index = stackIndex + 1;
                                stack.Insert(index, newCard);
                                mystack.Clear();
                                if (stack3.Count != 0)
                                    stack3[stack3.Count - 1].back = false;
                            }
                        }
                    }
                    if (selectedStack == 4)
                    {
                        if (mystack.Count > 0)
                        {
                            if (countStack != 0)
                            {
                                compareTo = stack[stackIndex].rank + 1;
                                compareTo2 = stack[stackIndex].suit;
                            }
                            else
                            {
                                compareTo = 1;
                                compareTo2 = mystack[mystack.Count - 1].suit;
                            }
                            if (mystack[mystack.Count - 1].rank == compareTo &&
                                mystack[mystack.Count - 1].suit == compareTo2)
                            {
                                s_matCard = false;
                                selectedPIndex = -1;
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = whichstack;
                                newCard.rank = compareTo;
                                newCard.suit = compareTo2;
                                int index = 0;
                                if (stackIndex == -1)
                                    index = 0;
                                else
                                    index = stackIndex + 1;
                                stack.Insert(index, newCard);
                                mystack.Clear();
                                if (stack4.Count != 0)
                                    stack4[stack4.Count - 1].back = false;
                            }
                        }
                    }
                    if (selectedStack == 5)
                    {
                        if (mystack.Count > 0)
                        {
                            if (countStack != 0)
                            {
                                compareTo = stack[stackIndex].rank + 1;
                                compareTo2 = stack[stackIndex].suit;
                            }
                            else
                            {
                                compareTo = 1;
                                compareTo2 = mystack[mystack.Count - 1].suit;
                            }
                            if (mystack[mystack.Count - 1].rank == compareTo &&
                                mystack[mystack.Count - 1].suit == compareTo2)
                            {
                                s_matCard = false;
                                selectedPIndex = -1;
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = whichstack;
                                newCard.rank = compareTo;
                                newCard.suit = compareTo2;
                                int index = 0;
                                if (stackIndex == -1)
                                    index = 0;
                                else
                                    index = stackIndex + 1;
                                stack.Insert(index, newCard);
                                mystack.Clear();
                                if (stack5.Count != 0)
                                    stack5[stack5.Count - 1].back = false;
                            }
                        }
                    }
                    if (selectedStack == 6)
                    {
                        if (mystack.Count > 0)
                        {
                            if (countStack != 0)
                            {
                                compareTo = stack[stackIndex].rank + 1;
                                compareTo2 = stack[stackIndex].suit;
                            }
                            else
                            {
                                compareTo = 1;
                                compareTo2 = mystack[mystack.Count - 1].suit;
                            }
                            if (mystack[mystack.Count - 1].rank == compareTo &&
                                mystack[mystack.Count - 1].suit == compareTo2)
                            {
                                s_matCard = false;
                                selectedPIndex = -1;
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = whichstack;
                                newCard.rank = compareTo;
                                newCard.suit = compareTo2;
                                int index = 0;
                                if (stackIndex == -1)
                                    index = 0;
                                else
                                    index = stackIndex + 1;
                                stack.Insert(index, newCard);
                                mystack.Clear();
                                if (stack6.Count != 0)
                                    stack6[stack6.Count - 1].back = false;
                            }
                        }
                    }
                    if (selectedStack == 7)
                    {
                        if (mystack.Count > 0)
                        {
                            if (countStack != 0)
                            {
                                compareTo = stack[stackIndex].rank + 1;
                                compareTo2 = stack[stackIndex].suit;
                            }
                            else
                            {
                                compareTo = 1;
                                compareTo2 = mystack[mystack.Count - 1].suit;
                            }
                            if (mystack[mystack.Count - 1].rank == compareTo &&
                                mystack[mystack.Count - 1].suit == compareTo2)
                            {
                                s_matCard = false;
                                selectedPIndex = -1;
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = whichstack;
                                newCard.rank = compareTo;
                                newCard.suit = compareTo2;
                                int index = 0;
                                if (stackIndex == -1)
                                    index = 0;
                                else
                                    index = stackIndex + 1;
                                stack.Insert(index, newCard);
                                mystack.Clear();
                                if (stack7.Count != 0)
                                    stack7[stack7.Count - 1].back = false;
                            }
                        }
                    }
                }

                if (selectStackPaste())
                {
                    s_potCard = false;

                    if (s_potCard)
                    {
                    }

                    selectedFourIndex = -1;

                    try
                    {
                        if(mystack.Count > 0)
                        {
                            if (selectedPaste == 1)
                            {
                                int ss = 0;
                                Boolean isit = false;
                                if (stack1.Count > 0)
                                {
                                    if (mystack[0].suit == Card.SPADE && (stack1[stack1.Count - 1].suit == Card.HEART || stack1[stack1.Count - 1].suit == Card.DIAMOND))
                                        isit = true;
                                    if (mystack[0].suit == Card.CLUB && (stack1[stack1.Count - 1].suit == Card.HEART || stack1[stack1.Count - 1].suit == Card.DIAMOND))
                                        isit = true;
                                    if (mystack[0].suit == Card.HEART && (stack1[stack1.Count - 1].suit == Card.SPADE || stack1[stack1.Count - 1].suit == Card.CLUB))
                                        isit = true;
                                    if (mystack[0].suit == Card.DIAMOND && (stack1[stack1.Count - 1].suit == Card.SPADE || stack1[stack1.Count - 1].suit == Card.CLUB))
                                        isit = true;
                                    if (stack1[stack1.Count - 1].back == true)
                                        isit = true;
                                    else if (mystack[0].rank != stack1[stack1.Count - 1].rank - 1)
                                        isit = false;
                                }
                                else if(selectedStack == 1 || mystack[0].card.Equals("king") || mystack[0].card.Equals("ace"))
                                    isit = true;
                                if (isit)
                                {
                                    while (ss < mystack.Count)
                                    {
                                        stack1.Add(mystack[ss]);
                                        mystack.Remove(mystack[ss]);

                                        stack1[stack1.Count - 1].whatplace = "stak1";

                                        mystackclear = true;
                                    }
                                    if (selectedS == 1)
                                    {
                                        stack1[stack1.Count - 1].back = false;
                                    }
                                    else if (selectedS == 2)
                                    {
                                        stack2[stack2.Count - 1].back = false;
                                    }
                                    else if (selectedS == 3)
                                    {
                                        stack3[stack3.Count - 1].back = false;
                                    }
                                    else if (selectedS == 4)
                                    {
                                        stack4[stack4.Count - 1].back = false;
                                    }
                                    else if (selectedS == 5)
                                    {
                                        stack5[stack5.Count - 1].back = false;
                                    }
                                    else if (selectedS == 6)
                                    {
                                        stack6[stack6.Count - 1].back = false;
                                    }
                                    else if (selectedS == 7)
                                    {
                                        stack7[stack7.Count - 1].back = false;
                                    }
                                }
                            }
                            if (selectedPaste == 2)
                            {
                                int ss = 0;
                                Boolean isit = false;
                                if (stack2.Count > 0)
                                {
                                    if (mystack[0].suit == Card.SPADE && (stack2[stack2.Count - 1].suit == Card.HEART || stack2[stack2.Count - 1].suit == Card.DIAMOND))
                                        isit = true;
                                    if (mystack[0].suit == Card.CLUB && (stack2[stack2.Count - 1].suit == Card.HEART || stack2[stack2.Count - 1].suit == Card.DIAMOND))
                                        isit = true;
                                    if (mystack[0].suit == Card.HEART && (stack2[stack2.Count - 1].suit == Card.SPADE || stack2[stack2.Count - 1].suit == Card.CLUB))
                                        isit = true;
                                    if (mystack[0].suit == Card.DIAMOND && (stack2[stack2.Count - 1].suit == Card.SPADE || stack2[stack2.Count - 1].suit == Card.CLUB))
                                        isit = true;
                                    if (stack2[stack2.Count - 1].back == true)
                                        isit = true;
                                    else if (mystack[0].rank != stack2[stack2.Count - 1].rank - 1)
                                        isit = false;
                                }
                                else if (selectedStack == 2 || mystack[0].card.Equals("king") || mystack[0].card.Equals("ace"))
                                    isit = true;
                                if (isit)
                                {
                                    while (ss < mystack.Count)
                                    {
                                        stack2.Add(mystack[ss]);
                                        mystack.Remove(mystack[ss]);

                                        stack2[stack2.Count - 1].whatplace = "stak2";

                                        mystackclear = true;
                                    }
                                    if (selectedS == 1)
                                    {
                                        stack1[stack1.Count - 1].back = false;
                                    }
                                    else if (selectedS == 2)
                                    {
                                        stack2[stack2.Count - 1].back = false;
                                    }
                                    else if (selectedS == 3)
                                    {
                                        stack3[stack3.Count - 1].back = false;
                                    }
                                    else if (selectedS == 4)
                                    {
                                        stack4[stack4.Count - 1].back = false;
                                    }
                                    else if (selectedS == 5)
                                    {
                                        stack5[stack5.Count - 1].back = false;
                                    }
                                    else if (selectedS == 6)
                                    {
                                        stack6[stack6.Count - 1].back = false;
                                    }
                                    else if (selectedS == 7)
                                    {
                                        stack7[stack7.Count - 1].back = false;
                                    }
                                }
                            }
                            if (selectedPaste == 3)
                            {
                                int ss = 0;
                                Boolean isit = false;
                                if (stack3.Count > 0)
                                {
                                    if (mystack[0].suit == Card.SPADE && (stack3[stack3.Count - 1].suit == Card.HEART || stack3[stack3.Count - 1].suit == Card.DIAMOND))
                                        isit = true;
                                    if (mystack[0].suit == Card.CLUB && (stack3[stack3.Count - 1].suit == Card.HEART || stack3[stack3.Count - 1].suit == Card.DIAMOND))
                                        isit = true;
                                    if (mystack[0].suit == Card.HEART && (stack3[stack3.Count - 1].suit == Card.SPADE || stack3[stack3.Count - 1].suit == Card.CLUB))
                                        isit = true;
                                    if (mystack[0].suit == Card.DIAMOND && (stack3[stack3.Count - 1].suit == Card.SPADE || stack3[stack3.Count - 1].suit == Card.CLUB))
                                        isit = true;
                                    if (stack3[stack3.Count - 1].back == true)
                                        isit = true;
                                    else if (mystack[0].rank != stack3[stack3.Count - 1].rank - 1)
                                        isit = false;
                                }
                                else if (selectedStack == 3 || mystack[0].card.Equals("king") || mystack[0].card.Equals("ace"))
                                    isit = true;
                                if (isit)
                                {
                                    while (ss < mystack.Count)
                                    {
                                        stack3.Add(mystack[ss]);
                                        mystack.Remove(mystack[ss]);

                                        stack3[stack3.Count - 1].whatplace = "stak3";

                                        mystackclear = true;
                                    }
                                    if (selectedS == 1)
                                    {
                                        stack1[stack1.Count - 1].back = false;
                                    }
                                    else if (selectedS == 2)
                                    {
                                        stack2[stack2.Count - 1].back = false;
                                    }
                                    else if (selectedS == 3)
                                    {
                                        stack3[stack3.Count - 1].back = false;
                                    }
                                    else if (selectedS == 4)
                                    {
                                        stack4[stack4.Count - 1].back = false;
                                    }
                                    else if (selectedS == 5)
                                    {
                                        stack5[stack5.Count - 1].back = false;
                                    }
                                    else if (selectedS == 6)
                                    {
                                        stack6[stack6.Count - 1].back = false;
                                    }
                                    else if (selectedS == 7)
                                    {
                                        stack7[stack7.Count - 1].back = false;
                                    }
                                }
                            }
                            if (selectedPaste == 4)
                            {
                                int ss = 0;
                                Boolean isit = false;
                                if (stack4.Count > 0)
                                {
                                    if (mystack[0].suit == Card.SPADE && (stack4[stack4.Count - 1].suit == Card.HEART || stack4[stack4.Count - 1].suit == Card.DIAMOND))
                                        isit = true;
                                    if (mystack[0].suit == Card.CLUB && (stack4[stack4.Count - 1].suit == Card.HEART || stack4[stack4.Count - 1].suit == Card.DIAMOND))
                                        isit = true;
                                    if (mystack[0].suit == Card.HEART && (stack4[stack4.Count - 1].suit == Card.SPADE || stack4[stack4.Count - 1].suit == Card.CLUB))
                                        isit = true;
                                    if (mystack[0].suit == Card.DIAMOND && (stack4[stack4.Count - 1].suit == Card.SPADE || stack4[stack4.Count - 1].suit == Card.CLUB))
                                        isit = true;
                                    if (stack4[stack4.Count - 1].back == true)
                                        isit = true;
                                    else if (mystack[0].rank != stack4[stack4.Count - 1].rank - 1)
                                        isit = false;
                                }
                                else if (selectedStack == 4 || mystack[0].card.Equals("king") || mystack[0].card.Equals("ace"))
                                    isit = true;
                                if (isit)
                                {
                                    while (ss < mystack.Count)
                                    {
                                        stack4.Add(mystack[ss]);
                                        mystack.Remove(mystack[ss]);

                                        stack4[stack4.Count - 1].whatplace = "stak4";

                                        mystackclear = true;
                                    }
                                    if (selectedS == 1)
                                    {
                                        stack1[stack1.Count - 1].back = false;
                                    }
                                    else if (selectedS == 2)
                                    {
                                        stack2[stack2.Count - 1].back = false;
                                    }
                                    else if (selectedS == 3)
                                    {
                                        stack3[stack3.Count - 1].back = false;
                                    }
                                    else if (selectedS == 4)
                                    {
                                        stack4[stack4.Count - 1].back = false;
                                    }
                                    else if (selectedS == 5)
                                    {
                                        stack5[stack5.Count - 1].back = false;
                                    }
                                    else if (selectedS == 6)
                                    {
                                        stack6[stack6.Count - 1].back = false;
                                    }
                                    else if (selectedS == 7)
                                    {
                                        stack7[stack7.Count - 1].back = false;
                                    }
                                }
                            }
                            if (selectedPaste == 5)
                            {
                                int ss = 0;
                                Boolean isit = false;
                                if (stack5.Count > 0)
                                {
                                    if (mystack[0].suit == Card.SPADE && (stack5[stack5.Count - 1].suit == Card.HEART || stack5[stack5.Count - 1].suit == Card.DIAMOND))
                                        isit = true;
                                    if (mystack[0].suit == Card.CLUB && (stack5[stack5.Count - 1].suit == Card.HEART || stack5[stack5.Count - 1].suit == Card.DIAMOND))
                                        isit = true;
                                    if (mystack[0].suit == Card.HEART && (stack5[stack5.Count - 1].suit == Card.SPADE || stack5[stack5.Count - 1].suit == Card.CLUB))
                                        isit = true;
                                    if (mystack[0].suit == Card.DIAMOND && (stack5[stack5.Count - 1].suit == Card.SPADE || stack5[stack5.Count - 1].suit == Card.CLUB))
                                        isit = true;
                                    if (stack5[stack5.Count - 1].back == true)
                                        isit = true;
                                    else if (mystack[0].rank != stack5[stack5.Count - 1].rank - 1)
                                        isit = false;
                                }
                                else if (selectedStack == 5 || mystack[0].card.Equals("king") || mystack[0].card.Equals("ace"))
                                    isit = true;
                                if (isit)
                                {
                                    while (ss < mystack.Count)
                                    {
                                        stack5.Add(mystack[ss]);
                                        mystack.Remove(mystack[ss]);

                                        stack5[stack5.Count - 1].whatplace = "stak5";

                                        mystackclear = true;
                                    }
                                    if (selectedS == 1)
                                    {
                                        stack1[stack1.Count - 1].back = false;
                                    }
                                    else if (selectedS == 2)
                                    {
                                        stack2[stack2.Count - 1].back = false;
                                    }
                                    else if (selectedS == 3)
                                    {
                                        stack3[stack3.Count - 1].back = false;
                                    }
                                    else if (selectedS == 4)
                                    {
                                        stack4[stack4.Count - 1].back = false;
                                    }
                                    else if (selectedS == 5)
                                    {
                                        stack5[stack5.Count - 1].back = false;
                                    }
                                    else if (selectedS == 6)
                                    {
                                        stack6[stack6.Count - 1].back = false;
                                    }
                                    else if (selectedS == 7)
                                    {
                                        stack7[stack7.Count - 1].back = false;
                                    }
                                }
                            }
                            if (selectedPaste == 6)
                            {
                                int ss = 0;
                                Boolean isit = false;
                                if (stack6.Count > 0)
                                {
                                    if (mystack[0].suit == Card.SPADE && (stack6[stack6.Count - 1].suit == Card.HEART || stack6[stack6.Count - 1].suit == Card.DIAMOND))
                                        isit = true;
                                    if (mystack[0].suit == Card.CLUB && (stack6[stack6.Count - 1].suit == Card.HEART || stack6[stack6.Count - 1].suit == Card.DIAMOND))
                                        isit = true;
                                    if (mystack[0].suit == Card.HEART && (stack6[stack6.Count - 1].suit == Card.SPADE || stack6[stack6.Count - 1].suit == Card.CLUB))
                                        isit = true;
                                    if (mystack[0].suit == Card.DIAMOND && (stack6[stack6.Count - 1].suit == Card.SPADE || stack6[stack6.Count - 1].suit == Card.CLUB))
                                        isit = true;
                                    if (stack6[stack6.Count - 1].back == true)
                                        isit = true;
                                    else if (mystack[0].rank != stack6[stack6.Count - 1].rank - 1)
                                        isit = false;
                                }
                                else if (selectedStack == 6 || mystack[0].card.Equals("king") || mystack[0].card.Equals("ace"))
                                    isit = true;
                                if (isit)
                                {
                                    while (ss < mystack.Count)
                                    {
                                        stack6.Add(mystack[ss]);
                                        mystack.Remove(mystack[ss]);

                                        stack6[stack6.Count - 1].whatplace = "stak6";

                                        mystackclear = true;
                                    }
                                    if (selectedS == 1)
                                    {
                                        stack1[stack1.Count - 1].back = false;
                                    }
                                    else if (selectedS == 2)
                                    {
                                        stack2[stack2.Count - 1].back = false;
                                    }
                                    else if (selectedS == 3)
                                    {
                                        stack3[stack3.Count - 1].back = false;
                                    }
                                    else if (selectedS == 4)
                                    {
                                        stack4[stack4.Count - 1].back = false;
                                    }
                                    else if (selectedS == 5)
                                    {
                                        stack5[stack5.Count - 1].back = false;
                                    }
                                    else if (selectedS == 6)
                                    {
                                        stack6[stack6.Count - 1].back = false;
                                    }
                                    else if (selectedS == 7)
                                    {
                                        stack7[stack7.Count - 1].back = false;
                                    }
                                }
                            }
                            if (selectedPaste == 7)
                            {
                                int ss = 0;
                                Boolean isit = false;
                                if (stack7.Count > 0)
                                {
                                    if (mystack[0].suit == Card.SPADE && (stack7[stack7.Count - 1].suit == Card.HEART || stack7[stack7.Count - 1].suit == Card.DIAMOND))
                                        isit = true;
                                    if (mystack[0].suit == Card.CLUB && (stack7[stack7.Count - 1].suit == Card.HEART || stack7[stack7.Count - 1].suit == Card.DIAMOND))
                                        isit = true;
                                    if (mystack[0].suit == Card.HEART && (stack7[stack7.Count - 1].suit == Card.SPADE || stack7[stack7.Count - 1].suit == Card.CLUB))
                                        isit = true;
                                    if (mystack[0].suit == Card.DIAMOND && (stack7[stack7.Count - 1].suit == Card.SPADE || stack7[stack7.Count - 1].suit == Card.CLUB))
                                        isit = true;
                                    if (stack7[stack7.Count - 1].back == true)
                                        isit = true;
                                    else if (mystack[0].rank != stack7[stack7.Count - 1].rank - 1)
                                        isit = false;
                                }
                                else if (selectedStack == 7 || mystack[0].card.Equals("king") || mystack[0].card.Equals("ace"))
                                    isit = true;
                                if (isit)
                                {
                                    while (ss < mystack.Count)
                                    {
                                        stack7.Add(mystack[ss]);
                                        mystack.Remove(mystack[ss]);

                                        stack7[stack7.Count - 1].whatplace = "stak7";

                                        mystackclear = true;
                                    }
                                    if (selectedS == 1)
                                    {
                                        stack1[stack1.Count - 1].back = false;
                                    }
                                    else if (selectedS == 2)
                                    {
                                        stack2[stack2.Count - 1].back = false;
                                    }
                                    else if (selectedS == 3)
                                    {
                                        stack3[stack3.Count - 1].back = false;
                                    }
                                    else if (selectedS == 4)
                                    {
                                        stack4[stack4.Count - 1].back = false;
                                    }
                                    else if (selectedS == 5)
                                    {
                                        stack5[stack5.Count - 1].back = false;
                                    }
                                    else if (selectedS == 6)
                                    {
                                        stack6[stack6.Count - 1].back = false;
                                    }
                                    else if (selectedS == 7)
                                    {
                                        stack7[stack7.Count - 1].back = false;
                                    }
                                }
                            }
                        }
                        selectedStackCard = false;
                    }
                    catch (Exception e)
                    {
                        String str = e.Message;
                    }
                }
            }

            if (mystackclear)
            {
                mystack.Clear();
            }

            if (selectedStackCard == false && selectPotCard())
            {
                if (selectedStack == -11 || selectedStack == -10)
                {
                }
                else
                {
                    selectedStack = -2;
                }
                selectedPotCard = true;
            }

            if (selectedPotCard)
            {
                try
                {
                    if (selectStackPaste())
                    {
                        int compareTo = -1;
                        compareTo = stack[selectedStackIndex].rank + 1;
                        if (selectedPaste == 1)
                        {
                            if (stack1[selectedPasteIndex].rank == compareTo)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = "stak1";
                                newCard.rank = stack[selectedStackIndex].rank;
                                newCard.suit = stack[selectedStackIndex].suit;
                                if (stack1.Count == 0)
                                    selectedPasteIndex = 0;
                                else
                                    selectedPasteIndex++;
                                stack1.Insert(selectedPasteIndex, newCard);
                                stack.Remove(stack[selectedStackIndex]);
                                if (stack1.Count != 0)
                                    stack1[stack1.Count - 1].back = false;
                            }
                        }
                        if (selectedPaste == 2)
                        {
                            if (stack2[selectedPasteIndex].rank == compareTo)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = "stak2";
                                newCard.rank = stack[selectedStackIndex].rank;
                                newCard.suit = stack[selectedStackIndex].suit;
                                if (stack2.Count == 0)
                                    selectedPasteIndex = 0;
                                else
                                    selectedPasteIndex++;
                                stack2.Insert(selectedPasteIndex, newCard);
                                stack.Remove(stack[selectedStackIndex]);
                                if (stack2.Count != 0)
                                    stack2[stack2.Count - 1].back = false;
                            }
                        }
                        if (selectedPaste == 3)
                        {
                            if (stack3[selectedPasteIndex].rank == compareTo)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = "stak3";
                                newCard.rank = stack[selectedStackIndex].rank;
                                newCard.suit = stack[selectedStackIndex].suit;
                                if (stack3.Count == 0)
                                    selectedPasteIndex = 0;
                                else
                                    selectedPasteIndex++;
                                stack3.Insert(selectedPasteIndex, newCard);
                                stack.Remove(stack[selectedStackIndex]);
                                if (stack3.Count != 0)
                                    stack3[stack3.Count - 1].back = false;
                            }
                        }
                        if (selectedPaste == 4)
                        {
                            if (stack4[selectedPasteIndex].rank == compareTo)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = "stak4";
                                newCard.rank = stack[selectedStackIndex].rank;
                                newCard.suit = stack[selectedStackIndex].suit;
                                if (stack4.Count == 0)
                                    selectedPasteIndex = 0;
                                else
                                    selectedPasteIndex++;
                                stack4.Insert(selectedPasteIndex, newCard);
                                stack.Remove(stack[selectedStackIndex]);
                                if (stack4.Count != 0)
                                    stack4[stack4.Count - 1].back = false;
                            }
                        }
                        if (selectedPaste == 5)
                        {
                            if (stack5[selectedPasteIndex].rank == compareTo)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = "stak5";
                                newCard.rank = stack[selectedStackIndex].rank;
                                newCard.suit = stack[selectedStackIndex].suit;
                                if (stack5.Count == 0)
                                    selectedPasteIndex = 0;
                                else
                                    selectedPasteIndex++;
                                stack5.Insert(selectedPasteIndex, newCard);
                                stack.Remove(stack[selectedStackIndex]);
                                if (stack5.Count != 0)
                                    stack5[stack5.Count - 1].back = false;
                            }
                        }
                        if (selectedPaste == 6)
                        {
                            if (stack6[selectedPasteIndex].rank == compareTo)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = "stak6";
                                newCard.rank = stack[selectedStackIndex].rank;
                                newCard.suit = stack[selectedStackIndex].suit;
                                if (stack6.Count == 0)
                                    selectedPasteIndex = 0;
                                else
                                    selectedPasteIndex++;
                                stack6.Insert(selectedPasteIndex, newCard);
                                stack.Remove(stack[selectedStackIndex]);
                                if (stack6.Count != 0)
                                    stack6[stack6.Count - 1].back = false;
                            }
                        }
                        if (selectedPaste == 7)
                        {
                            if (stack7[selectedPasteIndex].rank == compareTo)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = "stak7";
                                newCard.rank = stack[selectedStackIndex].rank;
                                newCard.suit = stack[selectedStackIndex].suit;
                                if (stack7.Count == 0)
                                    selectedPasteIndex = 0;
                                else
                                    selectedPasteIndex++;
                                stack7.Insert(selectedPasteIndex, newCard);
                                stack.Remove(stack[selectedStackIndex]);
                                if (stack7.Count != 0)
                                    stack7[stack7.Count - 1].back = false;
                            }
                        }
                        selectedPotCard = false;
                    }
                } catch(Exception e)
                {
                    String str = e.Message;
                }
            }

            if (selectedStackCard)
            {
                selectedAnywhere = false;
            }

            if (selectedAnywhere)
            {
                selectedStackCard = false;
            }

            if(!selectedStackCard)
            {
                selectedPotCard = selectPotCard();
            }

            if (selectedPotCard || continuePot) {
                continuePot = true;
                s_matCard = false;
                s_potCard = true;
                if (selectStackPaste()) {
                    continuePot = false;
                    selectedPotCard = false;
                    Card org = null, dest = null;
                    try
                    {
                        if (selectedPaste != -1)
                        {
                            if (selectedPasteIndex == -1)
                                selectedPasteIndex = 0;
                            if (selectedStack == 1)
                            {
                                org = stack[selectedStackIndex];
                            }
                            if (selectedStack == 2)
                            {
                                org = stack[selectedStackIndex];
                            }
                            if (selectedStack == 3)
                            {
                                org = stack[selectedStackIndex];
                            }
                            if (selectedStack == 4)
                            {
                                org = stack[selectedStackIndex];
                            }
                            if (selectedPaste == 1)
                            {
                                if (stack1.Count == 1)
                                {
                                    dest = stack1[0];
                                }
                                else
                                {
                                    if (selectedPasteIndex < stack1.Count)
                                        dest = stack1[selectedPasteIndex];
                                    else if (stack1.Count == 0)
                                        dest = null;
                                }
                            }
                            if (selectedPaste == 2)
                            {
                                if (stack2.Count == 1)
                                {
                                    dest = stack2[0];
                                }
                                else
                                {
                                    if (selectedPasteIndex < stack2.Count)
                                        dest = stack2[selectedPasteIndex];
                                    else if (stack2.Count == 0)
                                        dest = null;
                                }
                            }
                            if (selectedPaste == 3)
                            {
                                if (stack3.Count == 1)
                                {
                                    dest = stack3[0];
                                }
                                else
                                {
                                    if (selectedPasteIndex < stack3.Count)
                                        dest = stack3[selectedPasteIndex];
                                    else if (stack3.Count == 0)
                                        dest = null;
                                }
                            }
                            if (selectedPaste == 4)
                            {
                                if (stack4.Count == 1)
                                {
                                    dest = stack4[0];
                                }
                                else
                                {
                                    if (selectedPasteIndex < stack4.Count)
                                        dest = stack4[selectedPasteIndex];
                                    else if (stack4.Count == 0)
                                        dest = null;
                                }
                            }
                            if (selectedPaste == 5)
                            {
                                if (stack5.Count == 1)
                                {
                                    dest = stack5[0];
                                }
                                else
                                {
                                    if (selectedPasteIndex < stack5.Count)
                                        dest = stack5[selectedPasteIndex];
                                    else if (stack5.Count == 0)
                                        dest = null;
                                }
                            }
                            if (selectedPaste == 6)
                            {
                                if (stack6.Count == 1)
                                {
                                    dest = stack6[0];
                                }
                                else
                                {
                                    if (selectedPasteIndex < stack6.Count)
                                        dest = stack6[selectedPasteIndex];
                                    else if (stack6.Count == 0)
                                        dest = null;
                                }
                            }
                            if (selectedPaste == 7)
                            {
                                if (stack7.Count == 1)
                                {
                                    dest = stack7[0];
                                }
                                else
                                {
                                    if (selectedPasteIndex < stack7.Count)
                                        dest = stack7[selectedPasteIndex];
                                    else if (stack7.Count == 0)
                                        dest = null;
                                }
                            }
                            if (selectedPaste == 1)
                            {
                                if (stack1[stack1.Count - 1].rank == stack[selectedStackIndex].rank + 1)
                                {
                                    if (stack1[stack1.Count - 1].suit == Card.CLUB || stack1[stack1.Count - 1].suit == Card.SPADE)
                                    {
                                        if (stack[selectedStackIndex].suit == Card.DIAMOND || stack[selectedStackIndex].suit == Card.HEART)
                                        {
                                            stack1.Add(stack[selectedStackIndex]);
                                            stack.Remove(stack[selectedStackIndex]);
                                            stack1[stack1.Count - 1].whatplace = "stak1";
                                        }
                                    }
                                    else if (stack1[stack1.Count - 1].suit == Card.DIAMOND || stack1[stack1.Count - 1].suit == Card.HEART)
                                    {
                                        if (stack[selectedStackIndex].suit == Card.CLUB || stack[selectedStackIndex].suit == Card.SPADE)
                                        {
                                            stack1.Add(stack[selectedStackIndex]);
                                            stack.Remove(stack[selectedStackIndex]);
                                            stack1[stack1.Count - 1].whatplace = "stak1";
                                        }
                                    }
                                }
                            }
                            if (selectedPaste == 2)
                            {
                                if (stack2[stack2.Count - 1].rank == stack[selectedStackIndex].rank + 1)
                                {
                                    if (stack2[stack2.Count - 1].suit == Card.CLUB || stack2[stack2.Count - 1].suit == Card.SPADE)
                                    {
                                        if (stack[selectedStackIndex].suit == Card.DIAMOND || stack[selectedStackIndex].suit == Card.HEART)
                                        {
                                            stack2.Add(stack[selectedStackIndex]);
                                            stack.Remove(stack[selectedStackIndex]);
                                            stack2[stack2.Count - 1].whatplace = "stak2";
                                        }
                                    }
                                    else if (stack2[stack2.Count - 1].suit == Card.DIAMOND || stack2[stack2.Count - 1].suit == Card.HEART)
                                    {
                                        if (stack[selectedStackIndex].suit == Card.CLUB || stack[selectedStackIndex].suit == Card.SPADE)
                                        {
                                            stack2.Add(stack[selectedStackIndex]);
                                            stack.Remove(stack[selectedStackIndex]);
                                            stack2[stack2.Count - 1].whatplace = "stak2";
                                        }
                                    }
                                }
                            }
                            if (selectedPaste == 3)
                            {
                                if (stack3[stack3.Count - 1].rank == stack[selectedStackIndex].rank + 1)
                                {
                                    if (stack3[stack3.Count - 1].suit == Card.CLUB || stack3[stack3.Count - 1].suit == Card.SPADE)
                                    {
                                        if (stack[selectedStackIndex].suit == Card.DIAMOND || stack[selectedStackIndex].suit == Card.HEART)
                                        {
                                            stack3.Add(stack[selectedStackIndex]);
                                            stack.Remove(stack[selectedStackIndex]);
                                            stack3[stack3.Count - 1].whatplace = "stak3";
                                        }
                                    }
                                    else if (stack3[stack3.Count - 1].suit == Card.DIAMOND || stack3[stack3.Count - 1].suit == Card.HEART)
                                    {
                                        if (stack[selectedStackIndex].suit == Card.CLUB || stack[selectedStackIndex].suit == Card.SPADE)
                                        {
                                            stack3.Add(stack[selectedStackIndex]);
                                            stack.Remove(stack[selectedStackIndex]);
                                            stack3[stack3.Count - 1].whatplace = "stak3";
                                        }
                                    }
                                }
                            }
                            if (selectedPaste == 4)
                            {
                                if (stack4[stack4.Count - 1].rank == stack[selectedStackIndex].rank + 1)
                                {
                                    if (stack4[stack4.Count - 1].suit == Card.CLUB || stack4[stack4.Count - 1].suit == Card.SPADE)
                                    {
                                        if (stack[selectedStackIndex].suit == Card.DIAMOND || stack[selectedStackIndex].suit == Card.HEART)
                                        {
                                            stack4.Add(stack[selectedStackIndex]);
                                            stack.Remove(stack[selectedStackIndex]);
                                            stack4[stack4.Count - 1].whatplace = "stak4";
                                        }
                                    }
                                    else if (stack4[stack4.Count - 1].suit == Card.DIAMOND || stack4[stack4.Count - 1].suit == Card.HEART)
                                    {
                                        if (stack[selectedStackIndex].suit == Card.CLUB || stack[selectedStackIndex].suit == Card.SPADE)
                                        {
                                            stack4.Add(stack[selectedStackIndex]);
                                            stack.Remove(stack[selectedStackIndex]);
                                            stack4[stack4.Count - 1].whatplace = "stak4";
                                        }
                                    }
                                }
                            }
                            if (selectedPaste == 5)
                            {
                                if (stack5[stack5.Count - 1].suit == Card.CLUB || stack5[stack5.Count - 1].suit == Card.SPADE)
                                {
                                    if (stack[selectedStackIndex].suit == Card.DIAMOND || stack[selectedStackIndex].suit == Card.HEART)
                                    {
                                        stack5.Add(stack[selectedStackIndex]);
                                        stack.Remove(stack[selectedStackIndex]);
                                        stack5[stack5.Count - 1].whatplace = "stak5";
                                    }
                                }
                                else if (stack5[stack5.Count - 1].suit == Card.DIAMOND || stack5[stack5.Count - 1].suit == Card.HEART)
                                {
                                    if (stack[selectedStackIndex].suit == Card.CLUB || stack[selectedStackIndex].suit == Card.SPADE)
                                    {
                                        stack5.Add(stack[selectedStackIndex]);
                                        stack.Remove(stack[selectedStackIndex]);
                                        stack5[stack5.Count - 1].whatplace = "stak5";
                                    }
                                }
                            }
                            if (selectedPaste == 6)
                            {
                                if (stack6[stack6.Count - 1].rank == stack[selectedStackIndex].rank + 1)
                                {
                                    if (stack6[stack6.Count - 1].suit == Card.CLUB || stack6[stack6.Count - 1].suit == Card.SPADE)
                                    {
                                        if (stack[selectedStackIndex].suit == Card.DIAMOND || stack[selectedStackIndex].suit == Card.HEART)
                                        {
                                            stack6.Add(stack[selectedStackIndex]);
                                            stack.Remove(stack[selectedStackIndex]);
                                            stack6[stack6.Count - 1].whatplace = "stak6";
                                        }
                                    }
                                    else if (stack6[stack6.Count - 1].suit == Card.DIAMOND || stack6[stack6.Count - 1].suit == Card.HEART)
                                    {
                                        if (stack[selectedStackIndex].suit == Card.CLUB || stack[selectedStackIndex].suit == Card.SPADE)
                                        {
                                            stack6.Add(stack[selectedStackIndex]);
                                            stack.Remove(stack[selectedStackIndex]);
                                            stack6[stack6.Count - 1].whatplace = "stak6";
                                        }
                                    }
                                }
                            }
                            if (selectedPaste == 7)
                            {
                                if (stack7[stack7.Count - 1].rank == stack[selectedStackIndex].rank + 1)
                                {
                                    if (stack7[stack7.Count - 1].suit == Card.CLUB || stack7[stack7.Count - 1].suit == Card.SPADE)
                                    {
                                        if (stack[selectedStackIndex].suit == Card.DIAMOND || stack[selectedStackIndex].suit == Card.HEART)
                                        {
                                            stack7.Add(stack[selectedStackIndex]);
                                            stack.Remove(stack[selectedStackIndex]);
                                            stack7[stack7.Count - 1].whatplace = "stak7";
                                        }
                                    }
                                    else if (stack7[stack7.Count - 1].suit == Card.DIAMOND || stack7[stack7.Count - 1].suit == Card.HEART)
                                    {
                                        if (stack[selectedStackIndex].suit == Card.CLUB || stack[selectedStackIndex].suit == Card.SPADE)
                                        {
                                            stack7.Add(stack[selectedStackIndex]);
                                            stack.Remove(stack[selectedStackIndex]);
                                            stack7[stack7.Count - 1].whatplace = "stak7";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        String str = e.Message;
                    }
                }
            }

            base.Update(gameTime);
        }

        private bool selectStackCard()
        {
            try
            {
                var mousePosition1 = new Point(_currentMouseState.X, _currentMouseState.Y);

                if (showCursor == false)
                {
                    showCursor = false;
                    this.IsMouseVisible = true;
                }

                for (int i = 0; i < stack1.Count; i++)
                {
                    Rectangle someRectangle = stack1[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    if (i == stack1.Count - 1)
                        someRectangle.Height = 160;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition1))
                    {
                        if (stack1[i].back == false)
                        {
                            showCursor = true;
                            this.IsMouseVisible = false;
                        }
                    }
                }

                for (int i = 0; i < stack2.Count; i++)
                {
                    Rectangle someRectangle = stack2[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    if (i == stack2.Count - 1)
                        someRectangle.Height = 160;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition1))
                    {
                        if (stack2[i].back == false)
                        {
                            showCursor = true;
                            this.IsMouseVisible = false;
                        }
                    }
                }

                for (int i = 0; i < stack3.Count; i++)
                {
                    Rectangle someRectangle = stack3[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    if (i == stack3.Count - 1)
                        someRectangle.Height = 160;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition1))
                    {
                        if (stack3[i].back == false)
                        {
                            showCursor = true;
                            this.IsMouseVisible = false;
                        }
                    }
                }

                for (int i = 0; i < stack4.Count; i++)
                {
                    Rectangle someRectangle = stack4[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    if (i == stack4.Count - 1)
                        someRectangle.Height = 160;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition1))
                    {
                        if (stack4[i].back == false)
                        {
                            showCursor = true;
                            this.IsMouseVisible = false;
                        }
                    }
                }

                for (int i = 0; i < stack5.Count; i++)
                {
                    Rectangle someRectangle = stack5[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    if (i == stack5.Count - 1)
                        someRectangle.Height = 160;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition1))
                    {
                        if (stack5[i].back == false)
                        {
                            showCursor = true;
                            this.IsMouseVisible = false;
                        }
                    }
                }

                for (int i = 0; i < stack6.Count; i++)
                {
                    Rectangle someRectangle = stack6[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    if (i == stack6.Count - 1)
                        someRectangle.Height = 160;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition1))
                    {
                        if (stack6[i].back == false)
                        {
                            showCursor = true;
                            this.IsMouseVisible = false;
                        }
                    }
                }

                for (int i = 0; i < stack7.Count; i++)
                {
                    Rectangle someRectangle = stack7[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    if (i == stack7.Count - 1)
                        someRectangle.Height = 160;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition1))
                    {
                        if (stack7[i].back == false)
                        {
                            showCursor = true;
                            this.IsMouseVisible = false;
                        }
                    }
                }

                bool conttt = false;

                int ccnn1 = 0;

                int ccnn2 = 0;

                int ccnn3 = 0;

                int ccnn4 = 0;

                int ccnn5 = 0;

                int ccnn6 = 0;

                int ccnn7 = 0;

                if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    if (mystack == null)
                        mystack = new List<Card>();

                    if (cards == null)
                        return false;

                    var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                    for (int i = 0; i < stack1.Count;)
                    {
                        Rectangle someRectangle = stack1[i].place;
                        someRectangle.Width = 80;
                        someRectangle.Height = 30;

                        if (i == stack1.Count - 1)
                            someRectangle.Height = 160;

                        Rectangle area = someRectangle;

                        if (area.Contains(mousePosition) || conttt)
                        {
                            if (stack1[i].back == false)
                            {
                                ccnn1++;

                                conttt = true;

                                stkChanged = true;

                                selectedStack = 1;

                                selectedS = selectedStack;

                                selectedStackIndex = i;

                                mystack.Add(stack1[i]);

                                stack1.Remove(stack1[i]);
                            }
                            else
                            {
                                i++;
                                stkChanged = false;
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }
                    conttt = false;
                    for (int i = 0; i < stack2.Count;)
                    {
                        Rectangle someRectangle = stack2[i].place;
                        someRectangle.Width = 80;
                        someRectangle.Height = 30;

                        if (i == stack2.Count - 1)
                            someRectangle.Height = 160;

                        Rectangle area = someRectangle;

                        if (area.Contains(mousePosition) || conttt)
                        {
                            if (stack2[i].back == false)
                            {
                                ccnn2++;

                                conttt = true;

                                stkChanged = true;

                                selectedStack = 2;

                                selectedS = selectedStack;

                                selectedStackIndex = i;

                                mystack.Add(stack2[i]);

                                stack2.Remove(stack2[i]);
                            }
                            else
                            {
                                i++;
                                stkChanged = false;
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }
                    conttt = false;
                    for (int i = 0; i < stack3.Count;)
                    {
                        Rectangle someRectangle = stack3[i].place;
                        someRectangle.Width = 80;
                        someRectangle.Height = 30;

                        if (i == stack3.Count - 1)
                            someRectangle.Height = 160;

                        Rectangle area = someRectangle;

                        if (area.Contains(mousePosition) || conttt)
                        {
                            if (stack3[i].back == false)
                            {
                                ccnn3++;

                                conttt = true;

                                stkChanged = true;

                                selectedStack = 3;

                                selectedS = selectedStack;

                                selectedStackIndex = i;

                                mystack.Add(stack3[i]);

                                stack3.Remove(stack3[i]);
                            }
                            else
                            {
                                i++;
                                stkChanged = false;
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }
                    conttt = false;
                    for (int i = 0; i < stack4.Count;)
                    {
                        Rectangle someRectangle = stack4[i].place;
                        someRectangle.Width = 80;
                        someRectangle.Height = 30;

                        if (i == stack4.Count - 1)
                            someRectangle.Height = 160;

                        Rectangle area = someRectangle;

                        if (area.Contains(mousePosition) || conttt)
                        {
                            if (stack4[i].back == false)
                            {
                                ccnn4++;

                                conttt = true;

                                stkChanged = true;

                                selectedStack = 4;

                                selectedS = selectedStack;

                                selectedStackIndex = i;

                                mystack.Add(stack4[i]);

                                stack4.Remove(stack4[i]);
                            }
                            else
                            {
                                i++;
                                stkChanged = false;
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }
                    conttt = false;
                    for (int i = 0; i < stack5.Count;)
                    {
                        Rectangle someRectangle = stack5[i].place;
                        someRectangle.Width = 80;
                        someRectangle.Height = 30;

                        if (i == stack5.Count - 1)
                            someRectangle.Height = 160;

                        Rectangle area = someRectangle;

                        if (area.Contains(mousePosition) || conttt)
                        {
                            if (stack5[i].back == false)
                            {
                                ccnn5++;

                                conttt = true;

                                stkChanged = true;

                                selectedStack = 5;

                                selectedS = selectedStack;

                                selectedStackIndex = i;

                                mystack.Add(stack5[i]);

                                stack5.Remove(stack5[i]);
                            }
                            else
                            {
                                i++;
                                stkChanged = false;
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }
                    conttt = false;
                    for (int i = 0; i < stack6.Count;)
                    {
                        Rectangle someRectangle = stack6[i].place;
                        someRectangle.Width = 80;
                        someRectangle.Height = 30;

                        if (i == stack6.Count - 1)
                            someRectangle.Height = 160;

                        Rectangle area = someRectangle;

                        if (area.Contains(mousePosition) || conttt)
                        {
                            if (stack6[i].back == false)
                            {
                                ccnn6++;

                                conttt = true;

                                stkChanged = true;

                                selectedStack = 6;

                                selectedS = selectedStack;

                                selectedStackIndex = i;

                                mystack.Add(stack6[i]);

                                stack6.Remove(stack6[i]);
                            }
                            else
                            {
                                i++;
                                stkChanged = false;
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }
                    conttt = false;
                    for (int i = 0; i < stack7.Count;)
                    {
                        Rectangle someRectangle = stack7[i].place;
                        someRectangle.Width = 80;
                        someRectangle.Height = 30;

                        if (i == stack7.Count - 1)
                            someRectangle.Height = 160;

                        Rectangle area = someRectangle;

                        if (area.Contains(mousePosition) || conttt)
                        {
                            if (stack7[i].back == false)
                            {
                                conttt = true;

                                ccnn7++;

                                stkChanged = true;

                                selectedStack = 7;

                                selectedS = selectedStack;

                                selectedStackIndex = i;

                                mystack.Add(stack7[i]);

                                stack7.Remove(stack7[i]);
                            }
                            else
                            {
                                i++;
                                stkChanged = false;
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }
                }

                if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    return true;
                }
                else
                {
                    if (stkChanged && _previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        return true;

                    if (!selectStackPaste())
                    {
                        if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                        {
                            if (mystack.Count > 0)
                            {
                                if (selectedS == 1)
                                {
                                    for(int iii = 0; iii < mystack.Count; iii++)
                                        stack1.Add(mystack[iii]);
                                    mystack.Clear();
                                }
                                if (selectedS == 2)
                                {
                                    for (int iii = 0; iii < mystack.Count; iii++)
                                        stack2.Add(mystack[iii]);
                                    mystack.Clear();
                                }
                                if (selectedS == 3)
                                {
                                    for (int iii = 0; iii < mystack.Count; iii++)
                                        stack3.Add(mystack[iii]);
                                    mystack.Clear();
                                }
                                if (selectedS == 4)
                                {
                                    for (int iii = 0; iii < mystack.Count; iii++)
                                        stack4.Add(mystack[iii]);
                                    mystack.Clear();
                                }
                                if (selectedS == 5)
                                {
                                    for (int iii = 0; iii < mystack.Count; iii++)
                                        stack5.Add(mystack[iii]);
                                    mystack.Clear();
                                }
                                if (selectedS == 6)
                                {
                                    for (int iii = 0; iii < mystack.Count; iii++)
                                        stack6.Add(mystack[iii]);
                                    mystack.Clear();
                                }
                                if (selectedS == 7)
                                {
                                    for (int iii = 0; iii < mystack.Count; iii++)
                                        stack7.Add(mystack[iii]);
                                    mystack.Clear();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                        {
                            return true;
                        }
                    }

                    if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                        return false;

                    return false;
                }
            }
            catch (Exception e) {String str=e.Message;}

            if (stkChanged)
                return true;

            stkChanged = false;

            return false;
        }

        private bool selectPotCard()
        {
            Rectangle somRectangle1 = new Rectangle(300, 20, 80, 160);

            Rectangle are1 = somRectangle1;

            var mousePosition1 = new Point(_currentMouseState.X, _currentMouseState.Y);

            if (are1.Contains(mousePosition1))
            {
                showCursor = true;
                this.IsMouseVisible = false;
            }
            else
            {
                if (showCursor == false)
                {
                    showCursor = false;
                    this.IsMouseVisible = true;
                }
            }

            somRectangle1 = new Rectangle(400, 20, 80, 160);

            are1 = somRectangle1;

            if (are1.Contains(mousePosition1))
            {
                showCursor = true;
                this.IsMouseVisible = false;
            }
            else
            {
                if (showCursor == false)
                {
                    showCursor = false;
                    this.IsMouseVisible = true;
                }
            }

            somRectangle1 = new Rectangle(500, 20, 80, 160);

            are1 = somRectangle1;

            if (are1.Contains(mousePosition1))
            {
                showCursor = true;
                this.IsMouseVisible = false;
            }
            else
            {
                if (showCursor == false)
                {
                    showCursor = false;
                    this.IsMouseVisible = true;
                }
            }

            somRectangle1 = new Rectangle(600, 20, 80, 160);

            are1 = somRectangle1;

            if (are1.Contains(mousePosition1))
            {
                showCursor = true;
                this.IsMouseVisible = false;
            }
            else
            {
                if (showCursor == false)
                {
                    showCursor = false;
                    this.IsMouseVisible = true;
                }
            }

            if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (cards == null)
                    return false;

                countStack1 = 0; countStack2 = 0; countStack3 = 0; countStack4 = 0;

                selectedPIndex = selectedStackIndex;

                if(selectedStack > 0)
                    selectedS = selectedStack;

                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                for (int i = 0; i < stack.Count; i++)
                {
                    if (stack[i].whatplace.Equals("stack1"))
                    {
                        countStack1++;
                        insertStack1 = i;
                    }
                }

                for (int i = 0; i < stack.Count; i++)
                {
                    if (stack[i].whatplace.Equals("stack2"))
                    {
                        countStack2++;
                        insertStack2 = i;
                    }
                }

                for (int i = 0; i < stack.Count; i++)
                {
                    if (stack[i].whatplace.Equals("stack3"))
                    {
                        countStack3++;
                        insertStack3 = i;
                    }
                }

                for (int i = 0; i < stack.Count; i++)
                {
                    if (stack[i].whatplace.Equals("stack4"))
                    {
                        countStack4++;
                        insertStack4 = i;
                    }
                }

                Rectangle somRectangle = new Rectangle(300, 20, 80, 160);

                Rectangle are = somRectangle;

                if (are.Contains(mousePosition) && countStack1 != 0)
                {
                    selectedStack = -2;

                    selectedStackIndex = insertStack1;

                    selectedFourIndex = insertStack1;

                    selectedPotCard = true;

                    selectedMatCard = false;

                    if (selectedStackIndex < 0)
                        selectedStackIndex = 0;

                    return true;
                }

                somRectangle = new Rectangle(400, 20, 80, 160);

                are = somRectangle;

                if (are.Contains(mousePosition) && countStack2 != 0)
                {
                    selectedStack = -2;

                    selectedStackIndex = insertStack2;

                    selectedFourIndex = insertStack2;

                    selectedPotCard = true;

                    selectedMatCard = false;

                    if (selectedStackIndex < 0)
                        selectedStackIndex = 0;

                    return true;
                }

                somRectangle = new Rectangle(500, 20, 80, 160);

                are = somRectangle;

                if (are.Contains(mousePosition) && countStack3 != 0)
                {
                    selectedStack = -2;

                    selectedStackIndex = insertStack3;

                    selectedFourIndex = insertStack3;

                    selectedPotCard = true;

                    selectedMatCard = false;

                    if (selectedStackIndex < 0)
                        selectedStackIndex = 0;

                    return true;
                }

                somRectangle = new Rectangle(600, 20, 80, 160);

                are = somRectangle;

                if (are.Contains(mousePosition) && countStack4 != 0)
                {
                    selectedStack = -2;

                    selectedStackIndex = insertStack4;

                    selectedFourIndex = insertStack4;

                    selectedPotCard = true;

                    selectedMatCard = false;

                    if (selectedStackIndex < 0)
                        selectedStackIndex = 0;

                    return true;
                }
            }

            return false;
        }

        private void stakHoverTest()
        {
            try
            {
                stakHover = "";

                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                for (int i = 0; i < stack1.Count; i++)
                {
                    Rectangle someRectangle = stack1[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 160;

                    if (i < stack1.Count - 1)
                        someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        stakHover = "stak1";
                    }
                }
                for (int i = 0; i < stack2.Count; i++)
                {
                    Rectangle someRectangle = stack2[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 160;

                    if (i < stack2.Count - 1)
                        someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        stakHover = "stak2";
                    }
                }
                for (int i = 0; i < stack3.Count; i++)
                {
                    Rectangle someRectangle = stack3[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 160;

                    if (i < stack3.Count - 1)
                        someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        stakHover = "stak3";
                    }
                }
                for (int i = 0; i < stack4.Count; i++)
                {
                    Rectangle someRectangle = stack4[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 160;

                    if (i < stack4.Count - 1)
                        someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        stakHover = "stak4";
                    }
                }
                for (int i = 0; i < stack5.Count; i++)
                {
                    Rectangle someRectangle = stack5[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 160;

                    if (i < stack5.Count - 1)
                        someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        stakHover = "stak5";
                    }
                }
                for (int i = 0; i < stack6.Count; i++)
                {
                    Rectangle someRectangle = stack6[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 160;

                    if (i < stack6.Count - 1)
                        someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        stakHover = "stak6";
                    }
                }
                for (int i = 0; i < stack7.Count; i++)
                {
                    Rectangle someRectangle = stack7[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 160;

                    if (i < stack7.Count - 1)
                        someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        stakHover = "stak7";
                    }
                }
            }
            catch (Exception e) {String str = e.Message;}
        }

        private bool selectStackPaste()
        {
            if ((_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                ||
                (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released))
            {

                if (cards == null)
                    return false;

                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                Rectangle somRectangle = new Rectangle(200, 220, 80, 160);

                Rectangle are = somRectangle;

                if (are.Contains(mousePosition))
                {
                    selectedPaste = 1;

                    selectedPasteIndex = -1;

                    selectedPast = selectedPaste;

                    s_potCard = false;
                }

                somRectangle = new Rectangle(300, 220, 80, 160);

                are = somRectangle;

                if (are.Contains(mousePosition))
                {
                    selectedPaste = 2;

                    selectedPasteIndex = -1;

                    selectedPast = selectedPaste;

                    s_potCard = false;
                }

                somRectangle = new Rectangle(400, 220, 80, 160);

                are = somRectangle;

                if (are.Contains(mousePosition))
                {
                    selectedPaste = 3;

                    selectedPasteIndex = -1;

                    selectedPast = selectedPaste;

                    s_potCard = false;
                }

                somRectangle = new Rectangle(500, 220, 80, 160);

                are = somRectangle;

                if (are.Contains(mousePosition))
                {
                    selectedPaste = 4;

                    selectedPasteIndex = -1;

                    selectedPast = selectedPaste;

                    s_potCard = false;
                }

                somRectangle = new Rectangle(600, 220, 80, 160);

                are = somRectangle;

                if (are.Contains(mousePosition))
                {
                    selectedPaste = 5;

                    selectedPasteIndex = -1;

                    selectedPast = selectedPaste;

                    s_potCard = false;
                }

                somRectangle = new Rectangle(700, 220, 80, 160);

                are = somRectangle;

                if (are.Contains(mousePosition))
                {
                    selectedPaste = 6;

                    selectedPasteIndex = -1;

                    selectedPast = selectedPaste;

                    s_potCard = false;
                }

                somRectangle = new Rectangle(800, 220, 80, 160);

                are = somRectangle;

                if (are.Contains(mousePosition))
                {
                    selectedPaste = 7;

                    selectedPasteIndex = -1;

                    selectedPast = selectedPaste;

                    s_potCard = false;
                }

                for (int i = 0; i < stack1.Count; i++)
                {
                    Rectangle someRectangle = stack1[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    if (i == stack1.Count - 1)
                        someRectangle.Height = 160;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        if (!stack1[i].back)
                        {
                            selectedPaste = 1;

                            selectedPasteIndex = i;

                            selectedFourIndex = -1;

                            s_potCard = false;

                            return true;
                        }
                    }
                    else
                        selectedPaste = -1;
                }
                for (int i = 0; i < stack2.Count; i++)
                {
                    Rectangle someRectangle = stack2[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    if (i == stack2.Count - 1)
                        someRectangle.Height = 160;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        if (!stack2[i].back)
                        {
                            selectedPaste = 2;

                            selectedPasteIndex = i;

                            selectedFourIndex = -1;

                            s_potCard = false;

                            return true;
                        }
                    }
                    else
                        selectedPaste = -1;
                }
                for (int i = 0; i < stack3.Count; i++)
                {
                    Rectangle someRectangle = stack3[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    if (i == stack3.Count - 1)
                        someRectangle.Height = 160;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        if (!stack3[i].back)
                        {
                            selectedPaste = 3;

                            selectedPasteIndex = i;

                            selectedFourIndex = -1;

                            s_potCard = false;

                            return true;
                        }
                    }
                    else
                        selectedPaste = -1;
                }
                for (int i = 0; i < stack4.Count; i++)
                {
                    Rectangle someRectangle = stack4[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    if (i == stack4.Count - 1)
                        someRectangle.Height = 160;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        if (!stack4[i].back)
                        {
                            selectedPaste = 4;

                            selectedPasteIndex = i;

                            selectedFourIndex = -1;

                            s_potCard = false;

                            return true;
                        }
                    }
                    else
                        selectedPaste = -1;
                }
                for (int i = 0; i < stack5.Count; i++)
                {
                    Rectangle someRectangle = stack5[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    if (i == stack5.Count - 1)
                        someRectangle.Height = 160;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        if (!stack5[i].back)
                        {
                            selectedPaste = 5;

                            selectedPasteIndex = i;

                            selectedFourIndex = -1;

                            s_potCard = false;

                            return true;
                        }
                    }
                    else
                        selectedPaste = -1;
                }
                for (int i = 0; i < stack6.Count; i++)
                {
                    Rectangle someRectangle = stack6[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    if (i == stack6.Count - 1)
                        someRectangle.Height = 160;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        if (!stack6[i].back)
                        {
                            selectedPaste = 6;

                            selectedPasteIndex = i;

                            selectedFourIndex = -1;

                            s_potCard = false;

                            return true;
                        }
                    }
                    else
                        selectedPaste = -1;
                }
                for (int i = 0; i < stack7.Count; i++)
                {
                    Rectangle someRectangle = stack7[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    if (i == stack7.Count - 1)
                        someRectangle.Height = 160;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        if (!stack7[i].back)
                        {
                            selectedPaste = 7;

                            selectedPasteIndex = i;

                            selectedFourIndex = -1;

                            s_potCard = false;

                            return true;
                        }
                    }
                    else
                        selectedPaste = -1;
                }

                selectedPaste = selectedPast;

                selectedPIndex = selectedPasteIndex;

                if (selectedPaste != -1)
                    return true;

                if (selectedPaste == -1)
                    selectedPaste = selectedPast;

                if (selectedPaste != -1)
                    return true;
            }

            return false;
        }

        private bool selectMatCard()
        {
            var mousePosition1 = new Point(_currentMouseState.X, _currentMouseState.Y);

            Rectangle someRectangle1 = new Rectangle(160+5, 20, 80, 160);

            Rectangle area1 = someRectangle1;

            if (area1.Contains(mousePosition1))
            {
                showCursor = true;
                this.IsMouseVisible = false;
            }
            else
            {
                if (showCursor == false)
                {
                    showCursor = false;
                    this.IsMouseVisible = true;
                }
            }

            if (_currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (s_matCard && selectedStack == -11)
                {
                    selectedStack = -10;

                    return true;
                }

                if (s_matCard && selectedStack == -10)
                {
                    selectedStack = -10;

                    return true;
                }

                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                Rectangle someRectangle = new Rectangle(160+5, 20, 80, 160);

                Rectangle area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    selectedStack = -11;

                    s_matCard = true;

                    return true;
                }
            }
            else
            {
                s_matCard = false;

                selectedStack = -2;
            }

            return false;
        }

        private bool selectAnywhere()
        {
            if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (cards == null)
                    return false;

                s_matCard = false;

                s_potCard = false;

                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                Rectangle someRectangle = new Rectangle(0, 0, 700, 900);

                Rectangle area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    selectedPIndex = -1;

                    return true;
                }
            }

            return false;
        }

        private bool stackClick()
        {
            if ((_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && 
                _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                ||
                (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released))
            {
                if (cards == null)
                    return false;
                
                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                Rectangle someRectangle = new Rectangle(300, 20, 80, 160);

                Rectangle area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack1";
                    return true;
                }

                someRectangle = new Rectangle(400, 20, 80, 160);

                area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack2";
                    return true;
                }

                someRectangle = new Rectangle(500, 20, 80, 160);

                area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack3";
                    return true;
                }

                someRectangle = new Rectangle(600, 20, 80, 160);

                area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack4";
                    return true;
                }
            }

            return false;
        }

        private bool nexts()
        {
            var mousePosition1 = new Point(_currentMouseState.X, _currentMouseState.Y);

            Rectangle someRectangle1 = new Rectangle(80, 20, 80, 160);

            Rectangle area1 = someRectangle1;

            if (area1.Contains(mousePosition1))
            {
                showCursor = true;
                this.IsMouseVisible = false;
            }
            else
            {
                if (showCursor == false)
                {
                    showCursor = false;
                    this.IsMouseVisible = true;
                }
            }

            if (!gameOver)
                if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                    Rectangle someRectangle = new Rectangle(80, 20, 80, 160);

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        beginPlayClicked = true;
                        stkChanged = false;
                        clickNext = true;
                    }
                }

            if (beginPlayClicked)
                return true;
            else
                return false;
        }

        private bool close()
        {
            try
            {
                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                Rectangle someRectangle = new Rectangle(10, 720, 150, 25);

                Rectangle area = someRectangle;

                if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    if (area.Contains(mousePosition))
                    {
                        return true;
                    }
                }
            }
            catch(Exception e1)
            {
                string str = e1.Message;
            }

            return false;
        }

        private bool beginPlay()
        {
            var mousePosition1 = new Point(_currentMouseState.X, _currentMouseState.Y);

            Rectangle someRectangle1 = new Rectangle(20, 230, 170, 20);

            Rectangle area1 = someRectangle1;

            if (area1.Contains(mousePosition1))
                hoverPlay = true;
            else
                hoverPlay = false;

            try
            {
                if (area1.Contains(mousePosition1))
                {
                    showCursor = true;
                    this.IsMouseVisible = false;
                }
                else
                {
                    showCursor = false;
                    this.IsMouseVisible = true;
                }
            }
            catch(Exception e1)
            {
                string str = e1.Message;
            }

            if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                Rectangle someRectangle = new Rectangle(20, 230, 170, 20);

                Rectangle area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    beginPlayClicked = true;
                    hoverPlay = true;
                }
                else
                    hoverPlay = false;
            }

            if (beginPlayClicked)
            {
                gameOver = false;

                cards = new List<Card>();

                for (int suit = Card.DIAMOND; suit <= Card.SPADE; suit++)
                {
                    for (int rank = 1; rank <= 13; rank++)
                    {
                        Card card = new Card();
                        cards.Add(card);

                        card.rank = rank;
                        card.suit = suit;
                        card.place = new Rectangle(160+5, 20, 80, 160);

                        if (rank == 1)
                        {
                            card.card = "ace";
                        }
                        else if (rank == 2)
                        {
                            card.card = "two";
                        }
                        else if (rank == 3)
                        {
                            card.card = "three";
                        }
                        else if (rank == 4)
                        {
                            card.card = "four";
                        }
                        else if (rank == 5)
                        {
                            card.card = "five";
                        }
                        else if (rank == 6)
                        {
                            card.card = "six";
                        }
                        else if (rank == 7)
                        {
                            card.card = "seven";
                        }
                        else if (rank == 8)
                        {
                            card.card = "eight";
                        }
                        else if (rank == 9)
                        {
                            card.card = "nine";
                        }
                        else if (rank == 10)
                        {
                            card.card = "ten";
                        }
                        else if (rank == 11)
                        {
                            card.card = "jack";
                        }
                        else if (rank == 12)
                        {
                            card.card = "queen";
                        }
                        else if (rank == 13)
                        {
                            card.card = "king";
                        }
                    }
                }

                do
                {
                    cards = FisherYates.Shuffle(cards);
                } while (cards[0].rank == 1);

                cardsMat = new List<Card>();
                mystack = new List<Card>();
                stack1 = new List<Card>();
                stack2 = new List<Card>();
                stack3 = new List<Card>();
                stack4 = new List<Card>();
                stack5 = new List<Card>();
                stack6 = new List<Card>();
                stack7 = new List<Card>();
                stack = new List<Card>();

                return true;

            } else {
            
                return false;
            }
        }

        private void dealCards()
        {
            cards[0].whatplace = "mat";
            cards[0].place = new Rectangle(160+5, 20, 80, 160);
            cardsMat.Add(cards[0]);
            cards.Remove(cards[0]);

            int i = 0;
            cards[i].whatplace = "stak1";
            stack1.Add(cards[i]);
            cards.Remove(cards[i]);

            cards[i].back = true;
            stack2.Add(cards[i]);
            stack2[stack2.Count - 1].whatplace = "stak2";
            cards.Remove(cards[i]);
            stack2.Add(cards[i]);
            stack2[stack2.Count - 1].whatplace = "stak2";
            cards.Remove(cards[i]);

            cards[i].back = true;
            stack3.Add(cards[i]);
            stack3[stack3.Count - 1].whatplace = "stak3";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack3.Add(cards[i]);
            stack3[stack3.Count - 1].whatplace = "stak3";
            cards.Remove(cards[i]);
            stack3.Add(cards[i]);
            stack3[stack3.Count - 1].whatplace = "stak3";
            cards.Remove(cards[i]);

            cards[i].back = true;
            stack4.Add(cards[i]);
            stack4[stack4.Count - 1].whatplace = "stak4";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack4.Add(cards[i]);
            stack4[stack4.Count - 1].whatplace = "stak4";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack4.Add(cards[i]);
            stack4[stack4.Count - 1].whatplace = "stak4";
            cards.Remove(cards[i]);
            stack4.Add(cards[i]);
            stack4[stack4.Count - 1].whatplace = "stak4";
            cards.Remove(cards[i]);

            cards[i].back = true;
            stack5.Add(cards[i]);
            stack5[stack5.Count - 1].whatplace = "stak5";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack5.Add(cards[i]);
            stack5[stack5.Count - 1].whatplace = "stak5";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack5.Add(cards[i]);
            stack5[stack5.Count - 1].whatplace = "stak5";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack5.Add(cards[i]);
            stack5[stack5.Count - 1].whatplace = "stak5";
            cards.Remove(cards[i]);
            stack5.Add(cards[i]);
            stack5[stack5.Count - 1].whatplace = "stak5";
            cards.Remove(cards[i]);

            cards[i].back = true;
            stack6.Add(cards[i]);
            stack6[stack6.Count - 1].whatplace = "stak6";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack6.Add(cards[i]);
            stack6[stack6.Count - 1].whatplace = "stak6";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack6.Add(cards[i]);
            stack6[stack6.Count - 1].whatplace = "stak6";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack6.Add(cards[i]);
            stack6[stack6.Count - 1].whatplace = "stak6";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack6.Add(cards[i]);
            stack6[stack6.Count - 1].whatplace = "stak6";
            cards.Remove(cards[i]);
            stack6.Add(cards[i]);
            stack6[stack6.Count - 1].whatplace = "stak6";
            cards.Remove(cards[i]);

            cards[i].back = true;
            stack7.Add(cards[i]);
            stack7[stack7.Count - 1].whatplace = "stak7";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack7.Add(cards[i]);
            stack7[stack7.Count - 1].whatplace = "stak7";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack7.Add(cards[i]);
            stack7[stack7.Count - 1].whatplace = "stak7";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack7.Add(cards[i]);
            stack7[stack7.Count - 1].whatplace = "stak7";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack7.Add(cards[i]);
            stack7[stack7.Count - 1].whatplace = "stak7";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack7.Add(cards[i]);
            stack7[stack7.Count - 1].whatplace = "stak7";
            cards.Remove(cards[i]);
            stack7.Add(cards[i]);
            stack7[stack7.Count - 1].whatplace = "stak7";
            cards.Remove(cards[i]);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.Window.Title = "Solitaire Decked Out (" + hour + ":" + minute + ":" + seconds + ")";

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(bgg[ii], new Rectangle(0, 0, 900, 700), Color.DarkGray);

            spriteBatch.Draw(bg[iibgg], new Rectangle(700, 0, 200, 200), Color.White);

            if (stack != null)
            {
                if (stack.Count == 52)
                {
                    if (!stopTimer)
                        MessageBox.Show("Congratulations!  You Won!");
                    stopTimer = true;
                    spriteBatch.Draw(bg[iibgg], new Rectangle(0, 0, 900, 700), Color.White);
                    spriteBatch.DrawString(spriteFont, "You Won!", new Vector2(10, 300), Color.Black);
                    spriteBatch.DrawString(spriteFont, "You took " + hour + ":" + minute + ":" + seconds, new Vector2(10, 330), Color.Black);
                }
            }

            if (cntt == 0)
            {
                ii++;
            }

            if (cntt2 == 0)
            {
                iibgg++;
            }

            cntt++;

            cntt2++;

            if (cntt == 6)
                cntt = 0;

            if (cntt2 == 4)
                cntt2 = 0;

            if (ii == 24)
                ii = 0;

            if (iibgg == 137)
                iibgg = 0;

            spriteBatch.DrawString(spriteFont, "Close", new Vector2(10, 650), Color.Red);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(80, 20), new Vector2(160, 20), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(80, 180), new Vector2(160, 180), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(80, 20), new Vector2(80, 180), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(160, 20), new Vector2(160, 180), Color.YellowGreen, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(160+5, 20), new Vector2(240 + 5, 20), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(160 + 5, 180), new Vector2(240 + 5, 180), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(160 + 5, 20), new Vector2(160 + 5, 180), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(240 + 5, 20), new Vector2(240 + 5, 180), Color.YellowGreen, 1);
            
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(300, 20), new Vector2(380, 20), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(300, 180), new Vector2(380, 180), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(300, 20), new Vector2(300, 180), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(380, 20), new Vector2(380, 180), Color.YellowGreen, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(400, 20), new Vector2(480, 20), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(400, 180), new Vector2(480, 180), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(400, 20), new Vector2(400, 180), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(480, 20), new Vector2(480, 180), Color.YellowGreen, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(500, 20), new Vector2(580, 20), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(500, 180), new Vector2(580, 180), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(500, 20), new Vector2(500, 180), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(580, 20), new Vector2(580, 180), Color.YellowGreen, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(600, 20), new Vector2(680, 20), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(600, 180), new Vector2(680, 180), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(600, 20), new Vector2(600, 180), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(680, 20), new Vector2(680, 180), Color.YellowGreen, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(200 + 10, 220), new Vector2(280 + 10, 220), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(200 + 10, 380), new Vector2(280 + 10, 380), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(200 + 10, 220), new Vector2(200 + 10, 380), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(280 + 10, 220), new Vector2(280 + 10, 380), Color.YellowGreen, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(300 + 10, 220), new Vector2(380 + 10, 220), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(300 + 10, 380), new Vector2(380 + 10, 380), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(300 + 10, 220), new Vector2(300 + 10, 380), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(380 + 10, 220), new Vector2(380 + 10, 380), Color.YellowGreen, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(400 + 10, 220), new Vector2(480 + 10, 220), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(400 + 10, 380), new Vector2(480 + 10, 380), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(400 + 10, 220), new Vector2(400 + 10, 380), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(480 + 10, 220), new Vector2(480 + 10, 380), Color.YellowGreen, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(500 + 10, 220), new Vector2(580 + 10, 220), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(500 + 10, 380), new Vector2(580 + 10, 380), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(500 + 10, 220), new Vector2(500 + 10, 380), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(580 + 10, 220), new Vector2(580 + 10, 380), Color.YellowGreen, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(600 + 10, 220), new Vector2(680 + 10, 220), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(600 + 10, 380), new Vector2(680 + 10, 380), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(600 + 10, 220), new Vector2(600 + 10, 380), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(680 + 10, 220), new Vector2(680 + 10, 380), Color.YellowGreen, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(700 + 10, 220), new Vector2(780 + 10, 220), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(700 + 10, 380), new Vector2(780 + 10, 380), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(700 + 10, 220), new Vector2(700 + 10, 380), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(780 + 10, 220), new Vector2(780 + 10, 380), Color.YellowGreen, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(800 + 10, 220), new Vector2(880 + 10, 220), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(800 + 10, 380), new Vector2(880 + 10, 380), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(800 + 10, 220), new Vector2(800 + 10, 380), Color.YellowGreen, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(880 + 10, 220), new Vector2(880 + 10, 380), Color.YellowGreen, 1);

            spriteBatch.Draw(pot, new Rectangle(300, 20, 80, 160), Color.White);
            spriteBatch.Draw(pot, new Rectangle(400, 20, 80, 160), Color.White);
            spriteBatch.Draw(pot, new Rectangle(500, 20, 80, 160), Color.White);
            spriteBatch.Draw(pot, new Rectangle(600, 20, 80, 160), Color.White);

            if (stakHover.Equals("stak1"))
            {
                SpriteBatchEx.DrawLine(spriteBatch, new Vector2(210, 210), new Vector2(290, 210), Color.Gray, 1);
            }
            else if (stakHover.Equals("stak2"))
            {
                SpriteBatchEx.DrawLine(spriteBatch, new Vector2(310, 210), new Vector2(390, 210), Color.Gray, 1);
            }
            else if (stakHover.Equals("stak3"))
            {
                SpriteBatchEx.DrawLine(spriteBatch, new Vector2(410, 210), new Vector2(490, 210), Color.Gray, 1);
            }
            else if (stakHover.Equals("stak4"))
            {
                SpriteBatchEx.DrawLine(spriteBatch, new Vector2(510, 210), new Vector2(590, 210), Color.Gray, 1);
            }
            else if (stakHover.Equals("stak5"))
            {
                SpriteBatchEx.DrawLine(spriteBatch, new Vector2(610, 210), new Vector2(690, 210), Color.Gray, 1);
            }
            else if (stakHover.Equals("stak6"))
            {
                SpriteBatchEx.DrawLine(spriteBatch, new Vector2(710, 210), new Vector2(790, 210), Color.Gray, 1);
            }
            else if (stakHover.Equals("stak7"))
            {
                SpriteBatchEx.DrawLine(spriteBatch, new Vector2(810, 210), new Vector2(890, 210), Color.Gray, 1);
            }

            spriteBatch.DrawString(spriteFont, "By: OkelyKodely", new Vector2(170, 520), Color.DarkRed);
            spriteBatch.DrawString(spriteFont, "Email: dh.cho428@gmail.com", new Vector2(170, 580), Color.DarkRed);

            if (hoverPlay == false)
                spriteBatch.Draw(beginPlayT, new Rectangle(20, 200, 170, 80), Color.White);
            else
                spriteBatch.Draw(beginPlayT, new Rectangle(2, 200, 200, 100), Color.White);

            if (cards != null)
            {
                for (int i = 0; i < cards.Count; ++i)
                    spriteBatch.Draw(back, new Rectangle(80, 20, 80, 160), Color.White);
                for (int i = 0; !gameOver && i <= cardsMat.Count - 1 && cardsMat.Count > 0; i++)
                {
                    card = Content.Load<Texture2D>(cardsMat[i].suit + "-" + cardsMat[i].rank);
                    spriteBatch.Draw(card, cardsMat[i].place, Color.White);
                }
                int count1 = 0, count2 = 0, count3 = 0, count4 = 0, count5 = 0, count6 = 0, count7 = 0;
                for (int i = 0; i < stack1.Count && stack1.Count > 0; i++)
                {
                    if (stack1[i].whatplace.Equals("stak1"))
                    {
                        stack1[i].place = new Rectangle(210, 220 + count1++ * 25, 80, 160);
                        if (stack1[i].back)
                        {
                            spriteBatch.Draw(back, stack1[i].place, Color.White);
                        }
                        else
                        {
                            card = Content.Load<Texture2D>(stack1[i].suit + "-" + stack1[i].rank);
                            spriteBatch.Draw(card, stack1[i].place, Color.White);
                        }
                    }
                }
                for (int i = 0; i < stack2.Count && stack2.Count > 0; i++)
                {
                    if (stack2[i].whatplace.Equals("stak2"))
                    {
                        stack2[i].place = new Rectangle(310, 220 + count2++ * 25, 80, 160);
                        if (stack2[i].back)
                        {
                            spriteBatch.Draw(back, stack2[i].place, Color.White);
                        }
                        else
                        {
                            card = Content.Load<Texture2D>(stack2[i].suit + "-" + stack2[i].rank);
                            spriteBatch.Draw(card, stack2[i].place, Color.White);
                        }
                    }
                }
                for (int i = 0; i < stack3.Count && stack3.Count > 0; i++)
                {
                    if (stack3[i].whatplace.Equals("stak3"))
                    {
                        stack3[i].place = new Rectangle(410, 220 + count3++ * 25, 80, 160);
                        if (stack3[i].back)
                        {
                            spriteBatch.Draw(back, stack3[i].place, Color.White);
                        }
                        else
                        {
                            card = Content.Load<Texture2D>(stack3[i].suit + "-" + stack3[i].rank);
                            spriteBatch.Draw(card, stack3[i].place, Color.White);
                        }
                    }
                }
                for (int i = 0; i < stack4.Count && stack4.Count > 0; i++)
                {
                    if (stack4[i].whatplace.Equals("stak4"))
                    {
                        stack4[i].place = new Rectangle(510, 220 + count4++ * 25, 80, 160);
                        if (stack4[i].back)
                        {
                            spriteBatch.Draw(back, stack4[i].place, Color.White);
                        }
                        else
                        {
                            card = Content.Load<Texture2D>(stack4[i].suit + "-" + stack4[i].rank);
                            spriteBatch.Draw(card, stack4[i].place, Color.White);
                        }
                    }
                }
                for (int i = 0; i < stack5.Count && stack5.Count > 0; i++)
                {
                    if (stack5[i].whatplace.Equals("stak5"))
                    {
                        stack5[i].place = new Rectangle(610, 220 + count5++ * 25, 80, 160);
                        if (stack5[i].back)
                        {
                            spriteBatch.Draw(back, stack5[i].place, Color.White);
                        }
                        else
                        {
                            card = Content.Load<Texture2D>(stack5[i].suit + "-" + stack5[i].rank);
                            spriteBatch.Draw(card, stack5[i].place, Color.White);
                        }
                    }
                }
                for (int i = 0; i < stack6.Count && stack6.Count > 0; i++)
                {
                    if (stack6[i].whatplace.Equals("stak6"))
                    {
                        stack6[i].place = new Rectangle(710, 220 + count6++ * 25, 80, 160);
                        if (stack6[i].back)
                        {
                            spriteBatch.Draw(back, stack6[i].place, Color.White);
                        }
                        else
                        {
                            card = Content.Load<Texture2D>(stack6[i].suit + "-" + stack6[i].rank);
                            spriteBatch.Draw(card, stack6[i].place, Color.White);
                        }
                    }
                }
                for (int i = 0; i < stack7.Count && stack7.Count > 0; i++)
                {
                    if (stack7[i].whatplace.Equals("stak7"))
                    {
                        stack7[i].place = new Rectangle(810, 220 + count7++ * 25, 80, 160);
                        if (stack7[i].back)
                        {
                            spriteBatch.Draw(back, stack7[i].place, Color.White);
                        }
                        else
                        {
                            card = Content.Load<Texture2D>(stack7[i].suit + "-" + stack7[i].rank);
                            spriteBatch.Draw(card, stack7[i].place, Color.White);
                        }
                    }
                }
                for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                {
                    if (stack[i].whatplace.Equals("stack1"))
                    {
                        stack[i].place = new Rectangle(300, 20, 80, 160);
                        card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                        spriteBatch.Draw(card, stack[i].place, Color.White);
                    }
                }
                for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                {
                    if (stack[i].whatplace.Equals("stack2"))
                    {
                        stack[i].place = new Rectangle(400, 20, 80, 160);
                        card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                        spriteBatch.Draw(card, stack[i].place, Color.White);
                    }
                }
                for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                {
                    if (stack[i].whatplace.Equals("stack3"))
                    {
                        stack[i].place = new Rectangle(500, 20, 80, 160);
                        card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                        spriteBatch.Draw(card, stack[i].place, Color.White);
                    }
                }
                for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                {
                    if (stack[i].whatplace.Equals("stack4"))
                    {
                        stack[i].place = new Rectangle(600, 20, 80, 160);
                        card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                        spriteBatch.Draw(card, stack[i].place, Color.White);
                    }
                }
            }

            if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                Rectangle someRectangle = new Rectangle(300, 20, 80, 160);

                Rectangle area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack1";
                }

                someRectangle = new Rectangle(400, 20, 80, 160);

                area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack2";
                }

                someRectangle = new Rectangle(500, 20, 80, 160);

                area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack3";
                }

                someRectangle = new Rectangle(600, 20, 80, 160);

                area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack4";
                }
            }

            try
            {
                if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                    _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                if (selectedStack == -2 || selectedPIndex != -1)
                {
                    if (whichstack.Equals("stack1"))
                        for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                        {
                            if (stack[i].whatplace.Equals("stack1") && selectedFourIndex != -1)
                            {
                                stack[i].place = new Rectangle(300, 20, 80, 160);
                                card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                                spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y, 80, 160), Color.White);
                            }
                        }
                    if (whichstack.Equals("stack2"))
                        for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                        {
                            if (stack[i].whatplace.Equals("stack2") && selectedFourIndex != -1)
                            {
                                stack[i].place = new Rectangle(400, 20, 80, 160);
                                card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                                spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y, 80, 160), Color.White);
                            }
                        }
                    if (whichstack.Equals("stack3"))
                        for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                        {
                            if (stack[i].whatplace.Equals("stack3") && selectedFourIndex != -1)
                            {
                                stack[i].place = new Rectangle(500, 20, 80, 160);
                                card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                                spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y, 80, 160), Color.White);
                            }
                        }
                    if (whichstack.Equals("stack4"))
                        for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                        {
                            if (stack[i].whatplace.Equals("stack4") && selectedFourIndex != -1)
                            {
                                stack[i].place = new Rectangle(600, 20, 80, 160);
                                card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                                spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y, 80, 160), Color.White);
                            }
                        }
                }
            }
            catch (Exception e) {String str=e.Message;}

            if (mystack != null)
            if (mystack.Count > 0)
            {
                int yy = 0;
                for (int i = 0; i < mystack.Count && mystack.Count > 0; i++)
                {
                    card = Content.Load<Texture2D>(mystack[i].suit + "-" + mystack[i].rank);
                    spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y + yy, 80, 160), Color.White);
                    yy += 25;
                }
            }

            if (s_matCard && selectedStack != -2)
            {
                smat = true;
                try
                {
                    for (int i = 0; !gameOver && i <= cardsMat.Count - 1 && cardsMat.Count > 0; i++)
                    {
                        try
                        {
                            card = Content.Load<Texture2D>(cardsMat[i].suit + "-" + cardsMat[i].rank);
                            spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y, 80, 160), Color.White);
                        }
                        catch (Exception e)
                        {
                            String str = e.Message;
                        }
                    }
                }
                catch (Exception e)
                {
                    String str = e.Message;
                }
            }
            else
            {
                smat = false;
            }

            try
            {
                if (showCursor)
                {
                    spriteBatch.Draw(cursorTex, new Rectangle(currentMousePosition.X, currentMousePosition.Y, 40, 40), Color.White);
                }
            }
            catch(Exception e1)
            {
                string str = e1.Message;
            }

            currentMousePosition.X = _currentMouseState.X;
            currentMousePosition.Y = _currentMouseState.Y;

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}