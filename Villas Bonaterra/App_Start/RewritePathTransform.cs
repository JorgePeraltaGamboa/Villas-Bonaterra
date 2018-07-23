using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Optimization;

namespace VillasBonaterra.App_Start
{
    /// <summary>
    /// The rewrite path transform.
    /// </summary>
    public class RewritePathTransform : IItemTransform
    {
        #region Constants

        /// <summary>
        /// The path to change default.
        /// </summary>
        private const string PathToChangeDefault = "..";

        /// <summary>
        /// The reg ex url.
        /// </summary>
        private const string RegExUrl = @"url\(.*?'\)";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RewritePathTransform"/> class.
        /// </summary>
        /// <param name="newPath">
        /// The new path.
        /// </param>
        /// <param name="pathToChange">
        /// The path to change.
        /// </param>
        public RewritePathTransform(string newPath, string pathToChange = PathToChangeDefault)
        {
            this.PathToChange = pathToChange;
            this.NewPath = newPath;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the new path.
        /// </summary>
        private string NewPath { get; set; }

        /// <summary>
        /// Gets or sets the path to change.
        /// </summary>
        private string PathToChange { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The process.
        /// </summary>
        /// <param name="includedVirtualPath">
        /// The included virtual path.
        /// </param>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string Process(string includedVirtualPath, string input)
        {
           /* if (IsDebugEnabled())
            {
                return input;
            }
            */
            var regex = new Regex(
                RegExUrl,
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

            input = regex.Replace(
                input,
                (m) =>
                {
                    var stringToCHange = m.Value;
                    return stringToCHange.Replace(this.PathToChange, this.NewPath);
                });
            //EventLog.WriteEntry("AdminCecai", input,EventLogEntryType.Warning, 234);
            return input;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The is debug enabled.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsDebugEnabled()
        {
            var compilationSection = (CompilationSection)ConfigurationManager.GetSection(@"system.web/compilation");
            EventLog.WriteEntry("AdminCecai", compilationSection.Debug + "", EventLogEntryType.Warning, 234);
            return compilationSection.Debug;
        }

        #endregion
    }
}