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
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    internal class NfieldTranslationsService : INfieldTranslationsService, INfieldConnectionClientObject
    {
        #region INfieldTranslationsService Members

        /// <summary>
        /// See <see cref="INfieldTranslationsService.QueryAsync"/>
        /// </summary>
        public async Task<IQueryable<Translation>> QueryAsync(string surveyId, int languageId)
        {
            CheckSurveyId(surveyId);

            var resposneMesage = await Client.GetAsync(TranslationsApi(surveyId, languageId, null).AbsoluteUri);
            var responseAsString = await resposneMesage.Content.ReadAsStringAsync();
            var translationsList =
                await JsonConvert.DeserializeObjectAsync<List<Translation>>(responseAsString).FlattenExceptions();
            return translationsList.AsQueryable();
        }

        /// <summary>
        /// See <see cref="INfieldTranslationsService.AddAsync"/>
        /// </summary>
        public async Task<Translation> AddAsync(string surveyId, int languageId, Translation translation)
        {
            CheckSurveyId(surveyId);

            if (translation == null)
            {
                throw new ArgumentNullException("translation");
            }

            var resposneMesage =
                await Client.PostAsJsonAsync(TranslationsApi(surveyId, languageId, null).AbsoluteUri, translation);
            var responseAsString = await resposneMesage.Content.ReadAsStringAsync();
            return await JsonConvert.DeserializeObjectAsync<Translation>(responseAsString).FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldTranslationsService.RemoveAsync"/>
        /// </summary>
        public async Task RemoveAsync(string surveyId, int languageId, Translation translation)
        {
            CheckSurveyId(surveyId);

            if (translation == null)
            {
                throw new ArgumentNullException("translation");
            }

            await
                Client.DeleteAsync(TranslationsApi(surveyId, languageId, translation.Name).AbsoluteUri)
                    .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldTranslationsService.UpdateAsync"/>
        /// </summary>
        public async Task UpdateAsync(string surveyId, int languageId, Translation translation)
        {
            CheckSurveyId(surveyId);

            if (translation == null)
            {
                throw new ArgumentNullException("translation");
            }

            await
                Client.PutAsJsonAsync(TranslationsApi(surveyId, languageId, null).AbsoluteUri, translation)
                    .FlattenExceptions();
        }


        /// <summary>
        /// See <see cref="INfieldTranslationsService.DefaultTextsAsync"/>
        /// </summary>
        public Task<IQueryable<Translation>> DefaultTextsAsync
        {
            get
            {
                return Client.GetAsync(ConnectionClient.NfieldServerUri.AbsoluteUri + "DefaultTexts")
                             .ContinueWith(
                                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                             .ContinueWith(
                                 stringTask =>
                                 JsonConvert.DeserializeObject<List<Translation>>(stringTask.Result).AsQueryable())
                             .FlattenExceptions();
            }
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private static void CheckSurveyId(string surveyId)
        {
            if (surveyId == null)
                throw new ArgumentNullException("surveyId");
            if (surveyId.Trim().Length == 0)
                throw new ArgumentException("surveyId cannot be empty");
        }

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri TranslationsApi(string surveyId, int languageId, string translationName)
        {
            StringBuilder uriText = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            uriText.AppendFormat("Surveys/{0}/Languages/{1}/Translations",
                    surveyId, languageId);
            if (!string.IsNullOrEmpty(translationName))
                uriText.AppendFormat("/{0}", translationName);
            return new Uri(uriText.ToString());
        }

    }
}
