﻿//----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Table;
using Ujo.IpfsImage.Services;
using Ujo.IpfsImage.Storage;
using Wintellect.Azure.Storage.Table;

namespace Ujo.IpfsImage.WebJob
{
    public class Functions
    {
        public static async Task ProcessIpfsCoverImage([QueueTrigger("IpfsCoverImageProcessingQueue")] string ipfsImageHash, [Table("IpfsImageResized")] CloudTable tableBinding, TextWriter log)
        {
            log.WriteLine("Start job");
            
            log.WriteLine("Processing cover image");
            await ProcessResizeImageByWidth(ipfsImageHash, tableBinding, 200);
            //image sizes?
            //todo write to work cloud table
            log.WriteLine("Finished processing cover image");
        }

        //this could be refactor to a generic project not related to Ujo
        //other resizes not related to work could be in a another web job queues.
        public static async Task ProcessResizeImageByHeight(string ipfsImageHash, CloudTable ipfsImageResizedCloudTable, int height)
        {
            var service = new IpfsImageService(ConfigurationSettings.GetIpfsRPCUrl());
            var image = await service.ScaleImageByHeight(ipfsImageHash, height);
            var sizeKey = IpfsImageResized.GetHeightNewSizeKey(height);
            var node = await service.AddImage(image, ipfsImageHash + "_" + sizeKey, ImageFormat.Png);
            var entity = IpfsImageResized.Create(ipfsImageResizedCloudTable, ipfsImageHash, sizeKey,
                node.Hash.ToString());
            await entity.InsertOrReplaceAsync();
        }

        public static async Task ProcessResizeImageByWidth(string ipfsImageHash, CloudTable ipfsImageResizedCloudTable, int width)
        {
            var service = new IpfsImageService(ConfigurationSettings.GetIpfsRPCUrl());
            var image = await service.ScaleImageByWidth(ipfsImageHash, width);
            var sizeKey = IpfsImageResized.GetWidthNewSizeKey(width);
            var node = await service.AddImage(image, ipfsImageHash + "_" + sizeKey, ImageFormat.Png);
            var entity = IpfsImageResized.Create(ipfsImageResizedCloudTable, ipfsImageHash, sizeKey,
                node.Hash.ToString());
            await entity.InsertOrReplaceAsync();
        }
    }
}
