using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialMediaApp.Models;
using System.Threading;
using System.Globalization;

namespace SocialMediaApp.Controllers
{
    public class MessengerController : Controller
    {
        //
        // GET: /messenger/
        public ActionResult Messenger()
        {
            MessengerModel m = new MessengerModel();
            m.consId = new ArrayList();
            m.contacts = new ArrayList();
            m.conversations = new ArrayList();
            m.date = new ArrayList();
            m.notifications = new ArrayList();
            m.starter = new ArrayList();
            m.userOneMess = new ArrayList();
            m.userTwoMess = new ArrayList();

            m.userId = (Int32)Session["ownerid"];
            var con = new SOCIALMEDIA_DBEntities();
            m.headerm = new headerModel();
            m.headerm.owner =con.USERS.Find(m.userId) ;

            ///get friends list
            ArrayList friendsIds = new ArrayList();
            var friends = con.FOLLOWINGS.Where(f => f.USERID == m.userId && f.FR_CONFIRMED == true).Select(f => f.FOLLOWINGID);
            foreach (int id in friends)
            {
                friendsIds.Add(id);
            }
            var friends_ = con.FOLLOWINGS.Where(f => f.FOLLOWINGID == m.userId && f.FR_CONFIRMED == true).Select(f => f.USERID);
            foreach (int id in friends_)
            {
                friendsIds.Add(id);
            }

            foreach (int fr in friendsIds)
            {
                USER friend = con.USERS.Find(fr);
                m.contacts.Add(friend);
            }
            m = getConversations(m);

            return View(m);
        }
        public MessengerModel getConversations(MessengerModel m)
        {

            dbUtils db = new dbUtils();
            string sql = "select con_id, con_contactID ,con_notifications,con_starter from CONVERSATIONS where con_userID =" + m.userId;
            ArrayList[] data = new ArrayList[4];
            data = db.fetch(sql, 4);
            m.consId = data[0];
            var con = new SOCIALMEDIA_DBEntities();
            foreach (int contactID in data[1])
                m.conversations.Add( con.USERS.Find(contactID) );
            m.notifications = data[2];
            m.starter = data[3];
            return m;
        }

        public ActionResult newConversation(int userID, int contactID , ArrayList friends_list)
        {
            dbUtils db = new dbUtils();
            dbUtils db2 = new dbUtils();

            string sql = "BEGIN IF NOT EXISTS (select * from CONVERSATIONS where con_userID=" + userID + " and con_contactID=" + contactID + ")" +
            " BEGIN insert into CONVERSATIONS (con_userID,con_contactID,con_notifications,con_starter) values (" + userID + "," + contactID + ",0," + 1 + ");";
            sql += "insert into CONVERSATIONS (con_userID,con_contactID,con_notifications,con_starter) values (" + contactID + "," + userID + ",0," + 0 + "); END END";

            string sql2 = "BEGIN CREATE TABLE [" + userID + "_AND_" + contactID + "] (" +
                        "[m_id] int identity(1,1)," +
                        "[userOneMess] nvarchar(MAX)," +
                        "[userTwoMess] nvarchar(MAX)," +
                        "[messDate] nvarchar(MAX)); END";


            try
            {
                db.create(sql);

            }
            catch (Exception)
            {

                throw;
            }
            try
            {
                db2.create(sql2);

            }
            catch (Exception)
            {

            }

            return RedirectToAction("Messenger");
        }
        public ActionResult loadChat(int conID)
        {
            chatModel m = new chatModel();
            dbUtils db = new dbUtils();
            ArrayList[] data;
            string sql = "select con_userID,con_contactID,con_starter from CONVERSATIONS where con_id=" + conID;

            try { data = db.fetch(sql, 3); }
            catch (Exception) { throw; }

            string userID = data[0][0].ToString(),
                contactID = data[1][0].ToString();
            int starter = (Int32)data[2][0];


            dbUtils db2 = new dbUtils();
            ArrayList[] data2;
            string sql2;
            if (starter == 1)
                sql2 = "select userOneMess,userTwoMess,messDate from [" + userID + "_AND_" + contactID + "]";
            else
                sql2 = "select userOneMess,userTwoMess,messDate from [" + contactID + "_AND_" + userID + "]";

            try { data2 = db2.fetch(sql2, 3); }
            catch (Exception) { throw; }

            sql = "update CONVERSATIONS set con_notifications = 0 where con_id=" + conID;

            try { db.Update(sql); }
            catch (Exception) { throw; }

            m.userOneMess = data2[0];
            m.userTwoMess = data2[1];
            m.date = data2[2];
            m.starter = starter;
            m.conID = conID;

            return PartialView("chatHistory", m);
        }

        [HttpPost]
        public void sendMessage(string m, int conID)
        {
            dbUtils db = new dbUtils();
            ArrayList[] data;
            string sql = "select con_userID,con_contactID,con_starter from CONVERSATIONS where con_id=" + conID;
            data = db.fetch(sql, 3);
            string userID = data[0][0].ToString(),
                contactID = data[1][0].ToString();
            int starter = (Int32)data[2][0];

            dbUtils db2 = new dbUtils();
            string sql2, date;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            date = DateTime.Now.ToString();
            ArrayList[] param = new ArrayList[2];
            param[0] = new ArrayList(); param[1] = new ArrayList();
            param[0].Add("@msg"); param[1].Add(m);
            if (starter == 1)
                sql2 = "insert into [" + userID + "_AND_" + contactID + "] (userOneMess,userTwoMess,messDate) values (@msg,'','" + date + "')";
            else
                sql2 = "insert into [" + contactID + "_AND_" + userID + "] (userOneMess,userTwoMess,messDate) values ('',@msg,'" + date + "')";

            try { db2.create(sql2, param); }
            catch (Exception) { }

            sql = "update CONVERSATIONS set con_notifications = con_notifications + 1 where con_userID=" + contactID + " and con_contactID=" + userID;

            try { db.Update(sql); }
            catch (Exception) { throw; }
        }
    }
}