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

namespace Nfield.Models
{
    /// <summary>
    /// Holds the properties of a Survey response code
    /// </summary>
    public class SurveyResponseCode
    {
        /// <summary>
        /// Survey id the response code belongs to
        /// </summary>
        public string SurveyId { get; set; }

        /// <summary>
        /// User defined response code for the survey
        /// </summary>
        public int ResponseCode { get; set; }

        /// <summary>
        /// User defined description of the response code given
        /// </summary>
        public string ResponseCodeDescription { get; set; }

        /// <summary>
        /// Determines if the Response code is a definitive or not (IsFinal - true or false)
        /// </summary>
        public bool? IsDefinite { get; set; }

        /// <summary>
        /// Determines if response code is selectable by the interviewer
        /// </summary>
        public bool? IsSelectable { get; set; }

        /// <summary>
        /// Determines if the Response code is meant for an appointment or not (IsIntermediate - true or false)
        /// </summary>
        public bool? AllowAppointment { get; set; }

    }
}