namespace DurakGame
{
    /// <summary>
    /// This a utility class containing state names as constants.
    /// This lets us ensure that we always use the same name to refer to important states
    /// </summary>
    public static class Names
    {
        /// <summary>
        /// This is of type Card.
        /// </summary>
        public const string ATTACKING_CARD = "attacking_card";
        /// <summary>
        /// This is of type Card.
        /// </summary>
        public const string DEFENDING_CARD = "defending_card";
        /// <summary>
        /// The name of the current round of the throwing card. This is of type Int.
        /// </summary>
        public const string CURRENT_ROUND_THROWING = "current_round_throwing_card";
        /// <summary>
        /// The name of the current round of the defending card. This is of type Int.
        /// </summary>
        public const string CURRENT_ROUND_DEFENDING = "current_round_defending_card";
        /// <summary>
        /// The name of the attacking player's ID. This is of type Int.
        /// </summary>
        public const string ATTACKING_PLAYER = "attacking_player_id";
        /// <summary>
        /// The name of the defending player's ID. This is of type Int.
        /// </summary>
        public const string DEFENDING_PLAYER = "defending_player_id";
        /// <summary>
        /// This is of type Boolean.
        /// </summary>
        public const string IS_ATTACKING = "is_attacking";
        /// <summary>
        /// This is of type Boolean.
        /// </summary>
        public const string DEFENDER_HAVE_NOT_CARDS = "is_defender_have_not_cards";
        /// <summary>
        /// This is of type Listint.
        /// </summary>
        public const string THROWES_WITHOUT_CARDS = "throwers_without_cards";
        /// <summary>
        /// This is of type Boolean.
        /// </summary>
        public const string DEFENDER_FORFEIT = "defender_forfeit";
        /// <summary>
        /// This is of type Boolean.
        /// </summary>
        public const string DEFENDER_TIMER_FINISHED = "defender_counter_finished";
        /// <summary>
        /// This is of type DictionaryIntBool. Where int — id, bool — is the player forfeit.
        /// </summary>
        public const string THROWING_PLAYERS = "throwing_players_id_bool";
        /// <summary>
        /// The name of the discard pile. This is of type CardCollection.
        /// </summary>
        public const string DISCARD = "discard_pile";
        /// <summary>
        /// The name of the temporary discard pile. This is of type CardCollection.
        /// </summary>
        public const string TEMP_DISCARD = "temporary_discard_pile";
        /// <summary>
        /// The name of the trump card field. This is of type Card.
        /// </summary>
        public const string TRUMP_CARD = "trump_card";
        /// <summary>
        /// The name of the number of cards in deck field. This is of type Int.
        /// </summary>
        public const string DECK_COUNT = "cards_in_deck";
        /// <summary>
        /// The name of the source deck field. This is of type CardCollection.
        /// </summary>
        public const string DECK = "source_deck";
        /// <summary>
        /// The name of the game over field. This is of type Boolean.
        /// </summary>
        public const string IS_GAME_OVER = "game_over";
        /// <summary>
        /// This is of type Listint. Where int — id.
        /// </summary>
        public const string WINNING_PLAYERS = "winning_players";
        /// <summary>
        /// The name of the losing player's ID. This is of type Int.
        /// </summary>
        public const string LOSER_ID = "loser_id";
        /// <summary>
        /// This is of type Boolean.
        /// </summary>
        public const string IS_TIE = "is_tie";
        /// <summary>
        /// The name of the trump card used field. This is of type Boolean.
        /// </summary>
        public const string TRUMP_CARD_USED = "trump_card_used";
        /// <summary>
        /// How many cards can be thrown to the defending player. This is of type Int.
        /// </summary>
        public const string CARDS_CAN_THROWN = "cards_can_be_thrown";
        /// <summary>
        /// The name of the initial card count field. This is of type Int.
        /// </summary>
        public const string AMOUNT_INIT_CARDS = "amount_init_cards";
        /// <summary>
        /// The name of the card count of the defender field. This is of type Int.
        /// </summary>
        public const string AMOUNT_CARD_DEFENDER = "amount_card_defender";
        /// <summary>
        /// This is of type Boolean.
        /// </summary>
        public const string REMOVE_ALL_FORFEITS = "remove_all_forfeits";
        /// <summary>
        /// This is of type Int. Where int — id.
        /// </summary>
        public const string PLAYER_FORFEIT = "player_forfeit";
    }
}
