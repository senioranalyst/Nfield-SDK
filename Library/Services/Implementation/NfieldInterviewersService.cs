﻿//    This file is part of Nfield.SDK.
//
//    Nfield.SDK is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Nfield.SDK is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public License
//    along with Nfield.SDK.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldInterviewersService"/>
    /// </summary>
    internal class NfieldInterviewersService : INfieldInterviewersService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldInterviewersService

        /// <summary>
        /// See <see cref="INfieldInterviewersService.AddAsync"/>
        /// </summary>
        public async Task<Interviewer> AddAsync(Interviewer interviewer)
        {
            var resposneMesage = await Client.PostAsJsonAsync(InterviewersApi.AbsoluteUri, interviewer);
            var responseAsString = await resposneMesage.Content.ReadAsStringAsync();
            return await JsonConvert.DeserializeObjectAsync<Interviewer>(responseAsString).FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.RemoveAsync"/>
        /// </summary>
        public Task RemoveAsync(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException("interviewer");
            }

            return Client.DeleteAsync(InterviewersApi + interviewer.InterviewerId).FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.UpdateAsync"/>
        /// </summary>
        public async Task<Interviewer> UpdateAsync(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException("interviewer");
            }

            var updatedInterviewer = new UpdateInterviewer
                {
                    EmailAddress = interviewer.EmailAddress,
                    FirstName = interviewer.FirstName,
                    LastName = interviewer.LastName,
                    TelephoneNumber = interviewer.TelephoneNumber
                };

            var resposneMesage = await Client.PatchAsJsonAsync(InterviewersApi + interviewer.InterviewerId, updatedInterviewer);
            var responseAsString = await resposneMesage.Content.ReadAsStringAsync();
            return await JsonConvert.DeserializeObjectAsync<Interviewer>(responseAsString).FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.QueryAsync"/>
        /// </summary>
        public async Task<IQueryable<Interviewer>> QueryAsync()
        {
            var resposneMesage = await Client.GetAsync(InterviewersApi.AbsoluteUri);
            var responseAsString = await resposneMesage.Content.ReadAsStringAsync();
            var interviewersList =
                await JsonConvert.DeserializeObjectAsync<List<Interviewer>>(responseAsString).FlattenExceptions();
            return interviewersList.AsQueryable();
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.ChangePasswordAsync"/>
        /// </summary>
        public async Task<Interviewer> ChangePasswordAsync(Interviewer interviewer, string password)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException("interviewer");
            }

            var resposneMesage =
                await Client.PutAsJsonAsync(InterviewersApi + interviewer.InterviewerId, (object) new {Password = password});
            var responseAsString = await resposneMesage.Content.ReadAsStringAsync();
            return await JsonConvert.DeserializeObjectAsync<Interviewer>(responseAsString).FlattenExceptions();            
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri InterviewersApi
        {
            get { return new Uri(ConnectionClient.NfieldServerUri.AbsoluteUri + "interviewers/"); }
        }
    }

    internal class UpdateInterviewer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
    }
}