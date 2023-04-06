using System.Data.SqlClient;

public class ClientModel {
  public string? id_client { get; set; }
  public string? nom_client { get; set; }
  public string? id_region { get; set; }

  public static string findAllQuery() {
    string result = $@"
      SELECT 
        id_client,
        nom_client,
        nom_region
      FROM 
        clientRegion
    ";
    return result;
  }
  public static string[] findAllDataStructure(string id_client, string nom_client, string nom_region) {
    string[] result = {id_client, nom_client, nom_region};
    return result;
  }
  public static List<string[]> findAll() {
    Connect connect = new Connect();
    string query = findAllQuery();
    SqlConnection connection = connect.getConnection();
    SqlCommand command = new SqlCommand(query, connection);
    SqlDataReader reader = command.ExecuteReader();

    List<string[]> result = new List<string[]>();
    while (reader.Read()) {
      string[] data = findAllDataStructure(
        reader.GetString(0), // * id_client
        reader.GetString(1), // * nom_client
        reader.GetString(2) // * nom_region
      );
      result.Add(data);
    }

    reader.Close();
    connection.Close();
    return result;
  }

  public static string findOneQuery(string id_client) {
    string result = $@"
      SELECT
        c.id_client,
        c.nom_client,
        r.nom_region,
        r.frequence_region
      FROM
        client as c
        JOIN region as r on c.id_region = r.id_region
          WHERE id_client = '{id_client}'
    ";
    return result;
  }
  public static string[] findOneDataStructure(string id_client, string nom_client, string nom_region, string frequence_region) {
    string[] result = {id_client, nom_client, nom_region, frequence_region};
    return result;
  }
  public static string[] findOne(string id_client) {
    Connect connect = new Connect();
    string query = findOneQuery(id_client);
    SqlConnection connection = connect.getConnection();
    SqlCommand command = new SqlCommand(query, connection);
    SqlDataReader reader = command.ExecuteReader();

    string[] result = new string[4];
    while (reader.Read()) {
      string[] data = findOneDataStructure(
        reader.GetString(0), // * id_client
        reader.GetString(1), // * nom_client
        reader.GetString(2), // * nom_region
        reader.GetDouble(3).ToString() // * frequence_region
      );
      result = data;
    }

    reader.Close();
    connection.Close();
    return result;
  }

  public static string findAllSubscriptionsQuery(string id_client) {
    string result = $@"
      SELECT 
        * 
      FROM 
        abonnementClient 
        WHERE 
          id_client = '{id_client}' 
          ORDER BY 
            debut_abonnement DESC
    ";
    return result;
  }
  public static string[] findAllSubscriptionsDataStructure(string id_client, string id_bouquet, string id_abonnement, string nom_bouquet, string remise, string prix_bouquet, string date_abonnement, string debut_abonnement, string fin_abonnement) {
    string[] result = {id_client, id_bouquet, id_abonnement, nom_bouquet, remise, prix_bouquet, date_abonnement, debut_abonnement, fin_abonnement};
    return result;
  }
  public static List<string[]> findAllSubscriptions(string id_client) {
    Connect connect = new Connect();
    string query = findAllSubscriptionsQuery(id_client);
    SqlConnection connection = connect.getConnection();
    SqlCommand command = new SqlCommand(query, connection);
    SqlDataReader reader = command.ExecuteReader();

    List<string[]> result = new List<string[]>();
    while (reader.Read()) {
      string[] data = findAllSubscriptionsDataStructure(
        reader.GetString(0), // * id_client
        reader.GetString(3), // * id_bouquet
        reader.GetString(7), // * id_abonnement
        reader.GetString(4), // * nom_bouquet
        reader.GetDouble(6).ToString(), // * remise
        reader.GetDouble(8).ToString(), // * prix_bouquet
        reader.GetDateTime(9).ToString(), // * date_abonnement
        reader.GetDateTime(10).ToString(), // * debut_abonnement
        reader.GetDateTime(11).ToString() // * fin_abonnement
      );
      result.Add(data);
    }

    reader.Close();
    connection.Close();
    return result;
  }

  public static string findAllBunchesQuery(string id_client) {
    string result = $@"
      SELECT 
        bi.* 
      FROM 
        bouquetDisposParRegion AS bdpr 
        JOIN 
          bouquetInfos AS bi ON bdpr.id_bouquet = bi.id_bouquet  
          WHERE 
            id_region = (select id_region from client where id_client = '{id_client}') AND bi.isPerso = 0  
            OR 
            id_region = (select id_region from client where id_client = '{id_client}') AND bi.id_client = '{id_client}'
    ";
    return result;
  }
  public static string[] findAllBunchesDataStructure(string id_bouquet, string nom_bouquet, string prix, string prixAR, string remise) {
    string[] result = {id_bouquet, nom_bouquet, prixAR, prix, remise};
    return result;
  }
  public static List<string[]> findAllBunches(string id_client) {
    Connect connect = new Connect();
    string query = findAllBunchesQuery(id_client);
    SqlConnection connection = connect.getConnection();
    SqlCommand command = new SqlCommand(query, connection);
    SqlDataReader reader = command.ExecuteReader();

    List<string[]> result = new List<string[]>();
    while (reader.Read()) {
      string[] data = findAllBunchesDataStructure(
        reader.GetString(0), // * id_bouquet
        reader.GetString(1), // * nom_bouquet
        reader.GetDouble(5).ToString(), // * prix
        reader.GetDouble(6).ToString(), // * prixAR
        reader.GetDouble(4).ToString() // * remise
      );
      result.Add(data);
    }

    reader.Close();
    connection.Close();
    return result;
  }

  public static string findSubscriptionEndLaterThanTodayQuery(string id_client) {
    string result = $@"
      SELECT 
        MAX(fin_abonnement) AS fin 
      FROM 
        abonnement 
        WHERE 
          CONVERT(date, fin_abonnement) >= CONVERT(date, getdate()) AND id_client = '{id_client}' 
          GROUP BY 
            id_client
    ";
    return result;
  }
  public static string findSubscriptionEndLaterThanToday(string id_client) {
    Connect connect = new Connect();
    string query = findSubscriptionEndLaterThanTodayQuery(id_client);
    SqlConnection connection = connect.getConnection();
    SqlCommand command = new SqlCommand(query, connection);
    SqlDataReader reader = command.ExecuteReader();

    string result = null;
    while (reader.Read()) {
      result = reader.GetDateTime(0).ToString();
    }

    reader.Close();
    connection.Close();
    return result;
  }
  public static string renewalQuery(string id_client, string id_bouquet, string date, int nbMonth, bool todayDegin) {
    string nextAbo = Connect.nextid("abo", Connect.nextValue("id_abonnement"));
    int nbDays = nbMonth * 30;
    string result = "";
    if (todayDegin) {
      result = $@"
        INSERT INTO ABONNEMENT 
          (id_abonnement, id_client, id_bouquet, prix_bouquet, date_abonnement, debut_abonnement, fin_abonnement)
        VALUES
        (
          '{nextAbo}',
          '{id_client}',
          '{id_bouquet}',
          (SELECT prix FROM prixBouquetAvecRemise WHERE id_bouquet = '{id_bouquet}'),
          GETDATE(),
          GETDATE(),
          (SELECT DATEADD(day, {nbDays}, GETDATE()))
        )
      ";
    } else {
      result = $@"
        INSERT INTO ABONNEMENT 
          (id_abonnement, id_client, id_bouquet, prix_bouquet, date_abonnement, debut_abonnement, fin_abonnement)
        VALUES
        (
          '{nextAbo}',
          '{id_client}',
          '{id_bouquet}',
          (SELECT prix FROM prixBouquetAvecRemise WHERE id_bouquet = '{id_bouquet}'),
          GETDATE(),
          '{date}',
          (SELECT DATEADD(day, {nbDays}, '{date}'))
        )
      ";
    }
    return result;
  }
  public static string findPriceOfTheLastSubscriptionAndTheNewBunchQuery(string id_client, string id_bouquet, string date) {
    string result = $@"
      SELECT 
        prix_bouquet, 
        (SELECT prix FROM prixBouquetAvecRemise WHERE id_bouquet = '{id_bouquet}')
      FROM 
        abonnement
        WHERE 
          id_client = '{id_client}' and fin_abonnement = '{date}'
    ";
    return result;
  }
  public static float[] findPriceOfTheLastSubscriptionAndTheNewBunchDataStructure(float prix_bouquet, float prix) {
    float[] result = {prix_bouquet, prix};
    return result;
  }
  public static float[] findPriceOfTheLastSubscriptionAndTheNewBunch(string id_client, string id_bouquet, string date) {
    Connect connect = new Connect();
    string query = findPriceOfTheLastSubscriptionAndTheNewBunchQuery(id_client, id_bouquet, date);
    SqlConnection connection = connect.getConnection();
    SqlCommand command = new SqlCommand(query, connection);
    SqlDataReader reader = command.ExecuteReader();

    float[] result = new float[2];
    while (reader.Read()) {
      result = findPriceOfTheLastSubscriptionAndTheNewBunchDataStructure(
        (float) reader.GetDouble(0),
        (float) reader.GetDouble(1)
      );
    }

    reader.Close();
    connection.Close();
    return result;
  }
  public static int renewal(string id_client, string id_bouquet, int nbMonth) {
    string lastDate = findSubscriptionEndLaterThanToday(id_client);
    if (lastDate == null) {
      string query = renewalQuery(id_client, id_bouquet, lastDate, nbMonth, true);
      Connect.executeNonQuery(query);
      return 0;
    } 
    float[] prices = findPriceOfTheLastSubscriptionAndTheNewBunch(id_client, id_bouquet, lastDate);
    if (prices[0] <= prices[1]) {
      string query = renewalQuery(id_client, id_bouquet, lastDate, nbMonth, false);
      Connect.executeNonQuery(query);
      return 0;
    }

    return -1;
  }
}