﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace Microsoft.Crank.Models
{
    public class Source
    {
        /// <summary>
        /// The name of a branch, or a commit hash starting with '#'
        /// </summary>
        public string BranchOrCommit { get; set; } = "";
        public string Repository { get; set; }
        public string Project { get; set; }
        public bool InitSubmodules { get; set; }
        public string DockerFile { get; set; }
        public string DockerImageName { get; set; }
        public string DockerLoad { get; set; } // Relative to the docker folder
        public string DockerCommand { get; set; } // Optional command arguments for 'docker run'
        public string DockerContextDirectory { get; set; }
        public string DockerFetchPath { get; set; }
        public string LocalFolder { get; set; }

        public bool IsDocker()
        {
            return !String.IsNullOrEmpty(DockerFile) || !String.IsNullOrEmpty(DockerImageName);
        }

        public string GetNormalizedImageName()
        {
            // If DockerLoad option is used, the image must be set to the one used to build it
            if (!string.IsNullOrEmpty(DockerLoad))
            {
                return DockerImageName;
            }

            if (!string.IsNullOrEmpty(DockerImageName))
            {
                // If the docker image name already starts with benchmarks, reuse it
                // This prefix is used to clean any dangling container that would not have been stopped automatically
                if (DockerImageName.StartsWith("benchmarks_"))
                {
                    return DockerImageName;
                }
                else
                {
                    return $"benchmarks_{DockerImageName}".ToLowerInvariant();
                }
            }
            else
            {
                return $"benchmarks_{Path.GetFileNameWithoutExtension(DockerFile)}".ToLowerInvariant();
            }            
        }

        // When set, contains the location of the uploaded source code
        public Attachment SourceCode { get; set; }
    }
}
