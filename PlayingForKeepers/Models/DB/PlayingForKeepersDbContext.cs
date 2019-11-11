﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PlayingForKeepers.Models.DB.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PlayingForKeepers.Models.DB
{
    public partial class PlayingForKeepersDbContext : IdentityDbContext<ApplicationUser>
    {

        #region public properties
        public virtual DbSet<FF_Leagues> FF_Leagues { get; set; }
        #endregion



        #region default constructor method
        public PlayingForKeepersDbContext()
        {
        }
        #endregion



        #region overloaded constructor method
        public PlayingForKeepersDbContext(DbContextOptions<PlayingForKeepersDbContext> options)
            : base(options)
        {
        }
        #endregion



        #region OnModelCreating method
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FF_LeagueActivity>().HasNoKey();

        }
        #endregion


        #region AddLeague SP method  
        // Adds a league and returns a bool to the caller 
        public Task<bool> AddLeague(string leagueName, int leagueTeamsPossible, string leagueOwnerID, LeagueStatus leagueStatus)
        {

            //Initialize
            bool outputParamValue;

            try
            {
                SqlParameter inputParam1 = new SqlParameter("@LeagueName", leagueName);
                SqlParameter inputParam2 = new SqlParameter("@LeagueTeamsPossible", leagueTeamsPossible);
                SqlParameter inputParam3 = new SqlParameter("@LeagueOwnerID", leagueOwnerID);
                SqlParameter inputParam4 = new SqlParameter("@LeagueStatus", leagueStatus);

                SqlParameter outputParam1 = new SqlParameter("@OutParam", SqlDbType.Bit);
                outputParam1.Direction = ParameterDirection.Output;

                string sqlQuery = "EXEC [dbo].[FF_AddLeague] " + "@LeagueName" + "," + "@LeagueTeamsPossible" + "," + "@LeagueOwnerID" + "," + "@LeagueStatus" + "," + "@OutParam OUT";

                Database.ExecuteSqlRaw(sqlQuery, inputParam1, inputParam2, inputParam3, inputParam4, outputParam1);
                outputParamValue = (bool)outputParam1.Value;


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Task.FromResult(outputParamValue);
        }
        #endregion


        #region GeJoinedLeaguesAsync SP method
        // Gets a list of leagues and returns to the caller
        public async Task<List<FF_Leagues>> GetJoinedLeaguesAsync(string userId)
        {
            // Initialization.  
            List<FF_Leagues> lst = new List<FF_Leagues>();

            try
            {
                // Processing.  
                SqlParameter inputParam1 = new SqlParameter("@UserId", userId);
                string sqlQuery = "EXEC [dbo].[FF_GetJoinedLeagues] " + "@UserId";

                lst = await this.Set<FF_Leagues>().FromSqlRaw(sqlQuery, inputParam1).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return lst;
        }
        #endregion 



        #region GetLeaguesAsync SP method
        // Gets a list of leagues and returns to the caller
        public async Task<List<FF_Leagues>> GetLeaguesAsync()
        {
            // Initialization.  
            List<FF_Leagues> lst = new List<FF_Leagues>();

            try
            {
                // Processing.  
                string sqlQuery = "EXEC [dbo].[FF_GetLeagues] ";

                lst = await this.Set<FF_Leagues>().FromSqlRaw(sqlQuery).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return lst;
        }
        #endregion     



        #region JoinLeague SP method  
        // Adds a league and returns a bool to the caller 
        public Task<bool> JoinLeague(int leagueId, string userId)
        {

            ////Initialize
            bool outputParamValue;

            try
            {
                SqlParameter inputParam1 = new SqlParameter("@LeagueId", leagueId.ToString());
                SqlParameter inputParam2 = new SqlParameter("@UserId", userId);

                SqlParameter outputParam1 = new SqlParameter("@OutParam", SqlDbType.Bit);
                outputParam1.Direction = ParameterDirection.Output;

                string sqlQuery = "EXEC [dbo].[FF_JoinLeague] " + "@LeagueId" + "," + "@UserId" + "," + "@OutParam OUT";

                Database.ExecuteSqlRaw(sqlQuery, inputParam1, inputParam2, outputParam1);
                outputParamValue = (bool)outputParam1.Value;


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Task.FromResult(outputParamValue);


        }
        #endregion


        #region GetLeaguesAsync SP method
        // Gets a list of leagues and returns to the caller
        public async Task<List<FF_LeagueActivity>> GetLeagueActivityAsync(int leagueId)
        {
            // Initialization.  
            List<FF_LeagueActivity> lst = new List<FF_LeagueActivity>();

            try
            {
                // Processing. 
                SqlParameter inputParam1 = new SqlParameter("@LeagueId", leagueId.ToString());
                string sqlQuery = "EXEC [dbo].[FF_GetLeagueActivity] " + "@LeagueId";

                lst = await this.Set<FF_LeagueActivity>().FromSqlRaw(sqlQuery, inputParam1).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return lst;
        }
        #endregion  

    }
}
