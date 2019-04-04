-- MySQL dump 10.13  Distrib 5.5.34, for Linux (x86_64)
--
-- Host: localhost    Database: gmp
-- ------------------------------------------------------
-- Server version	5.5.34-32.0-log

--
-- Table structure for table `daily_reports`
--

CREATE TABLE `daily_reports` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `serNUM` char(10) NOT NULL DEFAULT '0',
  `mfDEV` char(3) NOT NULL DEFAULT '0',
  `typeDEV` char(3) NOT NULL DEFAULT '0',
  `chNUM` char(3) NOT NULL DEFAULT '0',
  `dt` datetime NOT NULL,
  `dVwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `dVst` float(8,1) NOT NULL DEFAULT '0.0',
  `dValwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `dValst` float(8,1) NOT NULL DEFAULT '0.0',
  `dVwrk_alwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `dVst_alwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `dVmeter` float(11,1) NOT NULL DEFAULT '0.0',
  `press` float(6,4) NOT NULL DEFAULT '0.0000',
  `temper` float(4,2) NOT NULL DEFAULT '0.00',
  `dKsg` float(5,4) NOT NULL DEFAULT '0.0000',
  `kkorr` float(6,4) NOT NULL DEFAULT '0.0000',
  `dNumWrCor` smallint(6) NOT NULL DEFAULT '0',
  `dfKgMPadFlag` tinyint(4) NOT NULL DEFAULT '0',
  `reserved` char(20) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `dt` (`dt`),
  KEY `idpx` (`idpx`),
  KEY `serNUM` (`serNUM`),
  KEY `uniq_device_idx` (`serNUM`,`mfDEV`,`typeDEV`,`chNUM`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `daily_reports_ugt`
--

CREATE TABLE `daily_reports_ugt` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `serNUM` char(10) NOT NULL DEFAULT '0',
  `mfDEV` char(3) NOT NULL DEFAULT '0',
  `typeDEV` char(3) NOT NULL DEFAULT '0',
  `chNUM` char(3) NOT NULL DEFAULT '0',
  `dt` datetime NOT NULL,
  `dVwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `dVst` float(8,1) NOT NULL DEFAULT '0.0',
  `dValwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `dValst` float(8,1) NOT NULL DEFAULT '0.0',
  `dVwrk_alwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `dVst_alwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `dVmeter` float(11,1) NOT NULL DEFAULT '0.0',
  `press` float(6,4) NOT NULL DEFAULT '0.0000',
  `temper` float(4,2) NOT NULL DEFAULT '0.00',
  `dKsg` float(5,4) NOT NULL DEFAULT '0.0000',
  `kkorr` float(6,4) NOT NULL DEFAULT '0.0000',
  `aDur` int(10) unsigned NOT NULL DEFAULT '0',
  `aDurQDmin` float(6,2) NOT NULL DEFAULT '0.00',
  `reserved58` char(3) NOT NULL DEFAULT '0',
  `dNumWrCor` smallint(6) NOT NULL DEFAULT '0',
  `dfKgMPadFlag` tinyint(4) NOT NULL DEFAULT '0',
  `dVmeterN` int(10) unsigned NOT NULL DEFAULT '0',
  `avDe` float(6,2) NOT NULL DEFAULT '0.00',
  `avH` float(6,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`id`),
  KEY `dt` (`dt`),
  KEY `idpx` (`idpx`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `daily_reports_uv_01`
--

CREATE TABLE `daily_reports_uv_01` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `serNUM` char(10) NOT NULL DEFAULT '0',
  `mfDEV` char(3) NOT NULL DEFAULT '0',
  `typeDEV` char(3) NOT NULL DEFAULT '0',
  `chNUM` char(3) NOT NULL DEFAULT '0',
  `dates` datetime NOT NULL,
  `dTmv` int(10) NOT NULL DEFAULT '0',
  `dTmPer` int(10) NOT NULL DEFAULT '0',
  `dTmin` int(10) NOT NULL DEFAULT '0',
  `dTm_dP` int(10) NOT NULL DEFAULT '0',
  `dP` float(8,2) NOT NULL DEFAULT '0.00',
  `dT` float(8,2) NOT NULL DEFAULT '0.00',
  `ddP` float(8,2) NOT NULL DEFAULT '0.00',
  `dVstd` float(10,2) NOT NULL DEFAULT '0.00',
  `dVmin` float(10,2) NOT NULL DEFAULT '0.00',
  `dVadd` float(10,2) NOT NULL DEFAULT '0.00',
  `dFPTst` tinyint(3) NOT NULL DEFAULT '0',
  `dFdPst` tinyint(3) NOT NULL DEFAULT '0',
  `dFAlarm` tinyint(3) NOT NULL DEFAULT '0',
  `dFPTalarm` tinyint(3) NOT NULL DEFAULT '0',
  `dFdPalarm` tinyint(3) NOT NULL DEFAULT '0',
  `reserved` char(12) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `dates` (`dates`),
  KEY `idpx` (`idpx`),
  KEY `uniq_device_idx` (`serNUM`,`mfDEV`,`typeDEV`,`chNUM`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `daily_reports_uv_02`
--

CREATE TABLE `daily_reports_uv_02` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `serNUM` char(10) NOT NULL DEFAULT '0',
  `mfDEV` char(3) NOT NULL DEFAULT '0',
  `typeDEV` char(3) NOT NULL DEFAULT '0',
  `chNUM` char(3) NOT NULL DEFAULT '0',
  `dates` datetime NOT NULL,
  `dTmv` int(10) NOT NULL DEFAULT '0',
  `dP` float(8,2) NOT NULL DEFAULT '0.00',
  `dT` float(8,2) NOT NULL DEFAULT '0.00',
  `dVwrk` float(10,2) NOT NULL DEFAULT '0.00',
  `dVstd` float(10,2) NOT NULL DEFAULT '0.00',
  `dVadd` float(10,2) NOT NULL DEFAULT '0.00',
  `dVmin` float(10,2) NOT NULL DEFAULT '0.00',
  `dFPTst` tinyint(3) NOT NULL DEFAULT '0',
  `dFAlarm` tinyint(3) NOT NULL DEFAULT '0',
  `dFPTalarm` tinyint(3) NOT NULL DEFAULT '0',
  `reserved` char(36) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `dates` (`dates`),
  KEY `idpx` (`idpx`),
  KEY `uniq_device_idx` (`serNUM`,`mfDEV`,`typeDEV`,`chNUM`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `emergency_reports`
--

CREATE TABLE `emergency_reports` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `serNUM` char(10) NOT NULL DEFAULT '0',
  `mfDEV` char(3) NOT NULL DEFAULT '0',
  `typeDEV` char(3) NOT NULL DEFAULT '0',
  `chNUM` char(3) NOT NULL DEFAULT '0',
  `aDatBeg` datetime NOT NULL,
  `aDatEnd` datetime NOT NULL,
  `aRepeat` smallint(5) unsigned NOT NULL DEFAULT '0',
  `aCodAl` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `aTimeAl` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `aVwrk` float(9,2) NOT NULL DEFAULT '0.00',
  `aVst` float(9,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`id`),
  KEY `dt` (`aDatBeg`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `hourly_reports`
--

CREATE TABLE `hourly_reports` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `serNUM` char(10) NOT NULL DEFAULT '0',
  `mfDEV` char(3) NOT NULL DEFAULT '0',
  `typeDEV` char(3) NOT NULL DEFAULT '0',
  `chNUM` char(3) NOT NULL DEFAULT '0',
  `dt` datetime NOT NULL,
  `hVwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `hVst` float(8,1) NOT NULL DEFAULT '0.0',
  `hValwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `hValst` float(8,1) NOT NULL DEFAULT '0.0',
  `hVwrk_alwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `hVst_alwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `hVmeter` float(11,1) NOT NULL DEFAULT '0.0',
  `press` float(6,4) NOT NULL DEFAULT '0.0000',
  `temper` float(4,2) NOT NULL DEFAULT '0.00',
  `hKsg` float(5,4) NOT NULL DEFAULT '0.0000',
  `kkorr` float(6,4) NOT NULL DEFAULT '0.0000',
  `hNumWrCor` smallint(6) NOT NULL DEFAULT '0',
  `hfKgMPahFlag` tinyint(4) NOT NULL DEFAULT '0',
  `reserved` char(20) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `dt` (`dt`),
  KEY `idpx` (`idpx`),
  KEY `serNUM` (`serNUM`),
  KEY `uniq_device_idx` (`serNUM`,`mfDEV`,`typeDEV`,`chNUM`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `hourly_reports_ugt`
--

CREATE TABLE `hourly_reports_ugt` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `serNUM` char(10) NOT NULL DEFAULT '0',
  `mfDEV` char(3) NOT NULL DEFAULT '0',
  `typeDEV` char(3) NOT NULL DEFAULT '0',
  `chNUM` char(3) NOT NULL DEFAULT '0',
  `dt` datetime NOT NULL,
  `hVwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `hVst` float(8,1) NOT NULL DEFAULT '0.0',
  `hValwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `hValst` float(8,1) NOT NULL DEFAULT '0.0',
  `hVwrk_alwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `hVst_alwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `hVmeter` float(11,1) NOT NULL DEFAULT '0.0',
  `press` float(6,4) NOT NULL DEFAULT '0.0000',
  `temper` float(4,2) NOT NULL DEFAULT '0.00',
  `hKsg` float(5,4) NOT NULL DEFAULT '0.0000',
  `kkorr` float(6,4) NOT NULL DEFAULT '0.0000',
  `aDur` float(6,4) NOT NULL DEFAULT '0.0000',
  `aDurQDmin` float(6,4) NOT NULL DEFAULT '0.0000',
  `reserved58` char(3) NOT NULL DEFAULT '0',
  `hNumWrCor` smallint(6) NOT NULL DEFAULT '0',
  `hfKgMPahFlag` tinyint(4) NOT NULL DEFAULT '0',
  `hVmeterN` tinyint(4) NOT NULL DEFAULT '0',
  `avDe` float(6,2) NOT NULL DEFAULT '0.00',
  `avH` float(6,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`id`),
  KEY `dt` (`dt`),
  KEY `idpx` (`idpx`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `hourly_reports_uv_01`
--

CREATE TABLE `hourly_reports_uv_01` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `serNUM` char(10) NOT NULL DEFAULT '0',
  `mfDEV` char(3) NOT NULL DEFAULT '0',
  `typeDEV` char(3) NOT NULL DEFAULT '0',
  `chNUM` char(3) NOT NULL DEFAULT '0',
  `dates` datetime NOT NULL,
  `hTmv` int(10) NOT NULL DEFAULT '0',
  `hTmPer` int(10) NOT NULL DEFAULT '0',
  `hTmin` int(10) NOT NULL DEFAULT '0',
  `hTm_dP` int(10) NOT NULL DEFAULT '0',
  `hP` float(8,2) NOT NULL DEFAULT '0.00',
  `hT` float(8,2) NOT NULL DEFAULT '0.00',
  `hdP` float(8,2) NOT NULL DEFAULT '0.00',
  `hVstd` float(10,2) NOT NULL DEFAULT '0.00',
  `hVmin` float(10,2) NOT NULL DEFAULT '0.00',
  `hVadd` float(10,2) NOT NULL DEFAULT '0.00',
  `hFPTst` tinyint(3) NOT NULL DEFAULT '0',
  `hFdPst` tinyint(3) NOT NULL DEFAULT '0',
  `hFAlarm` tinyint(3) NOT NULL DEFAULT '0',
  `hFPTalarm` tinyint(3) NOT NULL DEFAULT '0',
  `hFdPalarm` tinyint(3) NOT NULL DEFAULT '0',
  `reserved` char(12) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `dates` (`dates`),
  KEY `idpx` (`idpx`)
) ENGINE=InnoDB AUTO_INCREMENT=329139 DEFAULT CHARSET=utf8;

--
-- Table structure for table `hourly_reports_uv_02`
--

CREATE TABLE `hourly_reports_uv_02` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `serNUM` char(10) NOT NULL DEFAULT '0',
  `mfDEV` char(3) NOT NULL DEFAULT '0',
  `typeDEV` char(3) NOT NULL DEFAULT '0',
  `chNUM` char(3) NOT NULL DEFAULT '0',
  `dates` datetime NOT NULL,
  `hTmv` int(10) NOT NULL DEFAULT '0',
  `hP` float(8,2) NOT NULL DEFAULT '0.00',
  `hT` float(8,2) NOT NULL DEFAULT '0.00',
  `hVwrk` float(10,2) NOT NULL DEFAULT '0.00',
  `hVstd` float(10,2) NOT NULL DEFAULT '0.00',
  `hVadd` float(10,2) NOT NULL DEFAULT '0.00',
  `hFPTst` tinyint(3) NOT NULL DEFAULT '0',
  `hFAlarm` tinyint(3) NOT NULL DEFAULT '0',
  `hFPTalarm` tinyint(3) NOT NULL DEFAULT '0',
  `reserved` char(48) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `dates` (`dates`),
  KEY `idpx` (`idpx`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `intervention_reports`
--

CREATE TABLE `intervention_reports` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `serNUM` char(8) NOT NULL DEFAULT '0',
  `mfDEV` char(2) NOT NULL DEFAULT '0',
  `typeDEV` char(2) NOT NULL DEFAULT '0',
  `chNUM` char(3) NOT NULL DEFAULT '0',
  `dt` datetime NOT NULL,
  `WhoIntrv` char(2) NOT NULL DEFAULT '0',
  `ParamCode` smallint(3) NOT NULL DEFAULT '0',
  `TypeValue` smallint(3) NOT NULL DEFAULT '0',
  `OldValue` varchar(20) NOT NULL DEFAULT '0',
  `NewValue` varchar(20) NOT NULL DEFAULT '0',
  `FlagDim` smallint(3) NOT NULL DEFAULT '0',
  `FlagPoint` smallint(3) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `onehour_reports`
--

CREATE TABLE `onehour_reports` (
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `serNUM` char(10) NOT NULL DEFAULT '0',
  `mfDEV` char(3) NOT NULL DEFAULT '0',
  `typeDEV` char(3) NOT NULL DEFAULT '0',
  `chNUM` char(3) NOT NULL DEFAULT '0',
  `dt` datetime NOT NULL,
  `hVwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `hVst` float(8,1) NOT NULL DEFAULT '0.0',
  `hValwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `hValst` float(8,1) NOT NULL DEFAULT '0.0',
  `hVwrk_alwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `hVst_alwrk` float(8,1) NOT NULL DEFAULT '0.0',
  `hVmeter` float(11,1) NOT NULL DEFAULT '0.0',
  `press` float(6,4) NOT NULL DEFAULT '0.0000',
  `temper` float(4,2) NOT NULL DEFAULT '0.00',
  `hKsg` float(5,4) NOT NULL DEFAULT '0.0000',
  `kkorr` float(6,4) NOT NULL DEFAULT '0.0000',
  `hNumWrCor` smallint(6) NOT NULL DEFAULT '0',
  `hfKgMPahFlag` tinyint(4) NOT NULL DEFAULT '0',
  `reserved` char(20) NOT NULL DEFAULT '0',
  UNIQUE KEY `smt_dt` (`serNUM`,`mfDEV`,`typeDEV`,`chNUM`,`dt`),
  KEY `dt` (`dt`),
  KEY `idpx` (`idpx`),
  KEY `serNUM` (`serNUM`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `pp_reports`
--

CREATE TABLE `pp_reports` (
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `ppManufac` char(3) NOT NULL DEFAULT '0',
  `ppDevType` char(3) NOT NULL DEFAULT '0',
  `ppSerNum_Cor` char(10) NOT NULL DEFAULT '0',
  `ppDevChan` char(3) NOT NULL DEFAULT '0',
  `ppVersRTV_Top` char(10) NOT NULL DEFAULT '0',
  `ppVersRTV_Low` char(20) NOT NULL DEFAULT '0',
  `ppSerN_iMod` char(10) NOT NULL DEFAULT '0',
  `ppSDTI` char(10) NOT NULL DEFAULT '0',
  `ddIMEI` char(16) NOT NULL DEFAULT '0',
  `ddTelNum` char(10) NOT NULL DEFAULT '0',
  `ppEastLong` char(10) NOT NULL DEFAULT '0',
  `ppNorthWidth` char(10) NOT NULL DEFAULT '0',
  `ppManu` char(32) NOT NULL DEFAULT '0',
  `ppBran` char(32) NOT NULL DEFAULT '0',
  `ppInfX` char(32) NOT NULL DEFAULT '0',
  `ppNamC` char(32) NOT NULL DEFAULT '0',
  `ppTlg` char(3) NOT NULL DEFAULT '0',
  `ppSerC` char(10) NOT NULL DEFAULT '0',
  `ppPuls` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppQmax` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppQtrn` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppQmin` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppQtrs` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppQmis` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppAlg` char(10) NOT NULL DEFAULT '0',
  `ppDen` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppCO2` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppN2` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppTbas` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppPbas` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppPmin` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppPmx` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppPdf` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppTmin` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppTmax` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppTdf` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppQdfD` float(9,5) NOT NULL DEFAULT '0.00000',
  `ppW_SE` char(3) NOT NULL DEFAULT '0',
  `ppQmiA` char(3) NOT NULL DEFAULT '0',
  KEY `idpx` (`idpx`),
  KEY `ppSerNum_Cor` (`ppSerNum_Cor`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `pp_reports_uv_01`
--

CREATE TABLE `pp_reports_uv_01` (
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `ppManufac` char(3) NOT NULL DEFAULT '0',
  `ppDevType` char(3) NOT NULL DEFAULT '0',
  `ppSerNum_Cor` char(10) NOT NULL DEFAULT '0',
  `ppDevChan` char(3) NOT NULL DEFAULT '0',
  `ppVersRTV_Top` char(10) NOT NULL DEFAULT '0',
  `ppVersRTV_Low` char(20) NOT NULL DEFAULT '0',
  `ppSerN_iMod` char(10) NOT NULL DEFAULT '0',
  `ppSDTI` char(10) NOT NULL DEFAULT '0',
  `ppIMEI` char(16) NOT NULL DEFAULT '0',
  `ppTelNum` char(10) NOT NULL DEFAULT '0',
  `ppEastLong` char(10) NOT NULL DEFAULT '0',
  `ppNorthWidth` char(10) NOT NULL DEFAULT '0',
  `ppVersUn` smallint(5) NOT NULL DEFAULT '0',
  `ppNumModB` smallint(5) NOT NULL DEFAULT '0',
  `ppEnergyS` smallint(5) NOT NULL DEFAULT '0',
  `ppTlg` smallint(5) NOT NULL DEFAULT '0',
  `ppAlg` smallint(5) NOT NULL DEFAULT '0',
  `ppNum_dP` smallint(5) NOT NULL DEFAULT '0',
  `ppMode_dP` smallint(5) NOT NULL DEFAULT '0',
  `ppTypeP` smallint(5) NOT NULL DEFAULT '0',
  `ppSt_dP` smallint(5) NOT NULL DEFAULT '0',
  `ppStP` smallint(5) NOT NULL DEFAULT '0',
  `ppStT` smallint(5) NOT NULL DEFAULT '0',
  `pprMinP` smallint(5) NOT NULL DEFAULT '0',
  `pprMin_dP` smallint(5) NOT NULL DEFAULT '0',
  `pprHourDiaph` smallint(5) NOT NULL DEFAULT '0',
  `ppSteelDiaph` smallint(5) NOT NULL DEFAULT '0',
  `ppSteelPipe` smallint(5) NOT NULL DEFAULT '0',
  `ppZoneAlarm` smallint(5) NOT NULL DEFAULT '0',
  `ppRegSeasonTm` smallint(5) NOT NULL DEFAULT '0',
  `ppRegChang` smallint(5) NOT NULL DEFAULT '0',
  `ppNextDayDen` float(8,2) NOT NULL DEFAULT '0.00',
  `ppPbarNextDay` float(8,2) NOT NULL DEFAULT '0.00',
  `ppNextDay_N2` float(8,2) NOT NULL DEFAULT '0.00',
  `ppNextDayCO2` float(8,2) NOT NULL DEFAULT '0.00',
  `ppDen` float(8,2) NOT NULL DEFAULT '0.00',
  `ppPbar` float(8,2) NOT NULL DEFAULT '0.00',
  `ppN2` float(8,2) NOT NULL DEFAULT '0.00',
  `ppCO2` float(8,2) NOT NULL DEFAULT '0.00',
  `ppdP1max` float(8,2) NOT NULL DEFAULT '0.00',
  `ppdP2max` float(8,2) NOT NULL DEFAULT '0.00',
  `ppSet_dPmin` float(8,2) NOT NULL DEFAULT '0.00',
  `ppPmax` float(8,2) NOT NULL DEFAULT '0.00',
  `ppSetPmin` float(8,2) NOT NULL DEFAULT '0.00',
  `ppSetPmax` float(8,2) NOT NULL DEFAULT '0.00',
  `ppTmax` float(8,2) NOT NULL DEFAULT '0.00',
  `ppTmin` float(8,2) NOT NULL DEFAULT '0.00',
  `ppSetTmin` float(8,2) NOT NULL DEFAULT '0.00',
  `ppSetTmax` float(8,2) NOT NULL DEFAULT '0.00',
  `ppConstPmin` float(8,2) NOT NULL DEFAULT '0.00',
  `ppdPmin` float(8,2) NOT NULL DEFAULT '0.00',
  `ppCutOff_dP1` float(8,2) NOT NULL DEFAULT '0.00',
  `ppCutOff_dP2` float(8,2) NOT NULL DEFAULT '0.00',
  `ppRadiusNarrow` float(8,2) NOT NULL DEFAULT '0.00',
  `ppAverSherPipe` float(8,2) NOT NULL DEFAULT '0.00',
  `ppTmDiaph` float(8,2) NOT NULL DEFAULT '0.00',
  `ppDiamNarrow` float(8,2) NOT NULL DEFAULT '0.00',
  `ppDiamPipe` float(8,2) NOT NULL DEFAULT '0.00',
  `ppAreaAlarmMet` float(8,2) NOT NULL DEFAULT '0.00',
  `ppConst_dP` float(8,2) NOT NULL DEFAULT '0.00',
  `ppConstP` float(8,2) NOT NULL DEFAULT '0.00',
  `ppConstT` float(8,2) NOT NULL DEFAULT '0.00',
  KEY `idpx` (`idpx`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `pp_reports_uv_02`
--

CREATE TABLE `pp_reports_uv_02` (
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `ppManufac` char(3) NOT NULL DEFAULT '0',
  `ppDevType` char(3) NOT NULL DEFAULT '0',
  `ppSerNum_Cor` char(10) NOT NULL DEFAULT '0',
  `ppDevChan` char(3) NOT NULL DEFAULT '0',
  `ppVersRTV_Top` char(10) NOT NULL DEFAULT '0',
  `ppVersRTV_Low` char(20) NOT NULL DEFAULT '0',
  `ppSerN_iMod` char(10) NOT NULL DEFAULT '0',
  `ppSDTI` char(10) NOT NULL DEFAULT '0',
  `ppIMEI` char(16) NOT NULL DEFAULT '0',
  `ppTelNum` char(10) NOT NULL DEFAULT '0',
  `ppEastLong` char(10) NOT NULL DEFAULT '0',
  `ppNorthWidth` char(10) NOT NULL DEFAULT '0',
  `ppVersUn` smallint(5) NOT NULL DEFAULT '0',
  `ppNumModB` smallint(5) NOT NULL DEFAULT '0',
  `ppTlg` smallint(5) NOT NULL DEFAULT '0',
  `ppEcBat` smallint(5) NOT NULL DEFAULT '0',
  `ppRegSeasonTm` smallint(5) NOT NULL DEFAULT '0',
  `ppRegChang` smallint(5) NOT NULL DEFAULT '0',
  `ppDenNextDay` float(8,2) NOT NULL DEFAULT '0.00',
  `ppPbarNextDay` float(8,2) NOT NULL DEFAULT '0.00',
  `ppNextDay_N2` float(8,2) NOT NULL DEFAULT '0.00',
  `ppNextDayCO2` float(8,2) NOT NULL DEFAULT '0.00',
  `ppDen` float(8,2) NOT NULL DEFAULT '0.00',
  `ppPbar` float(8,2) NOT NULL DEFAULT '0.00',
  `ppN2` float(8,2) NOT NULL DEFAULT '0.00',
  `ppCO2` float(8,2) NOT NULL DEFAULT '0.00',
  `ppSostLine` smallint(5) NOT NULL DEFAULT '0',
  `ppVmetStart` smallint(5) NOT NULL DEFAULT '0',
  `ppSensImp` smallint(5) NOT NULL DEFAULT '0',
  `ppAlg` smallint(5) NOT NULL DEFAULT '0',
  `ppRegQ` smallint(5) NOT NULL DEFAULT '0',
  `ppStP` smallint(5) NOT NULL DEFAULT '0',
  `ppStT` smallint(5) NOT NULL DEFAULT '0',
  `ppPmax` float(8,2) NOT NULL DEFAULT '0.00',
  `ppPmin` float(8,2) NOT NULL DEFAULT '0.00',
  `ppSetPmax` float(8,2) NOT NULL DEFAULT '0.00',
  `ppTmin` float(8,2) NOT NULL DEFAULT '0.00',
  `ppSetTmin` float(8,2) NOT NULL DEFAULT '0.00',
  `ppSetTmax` float(8,2) NOT NULL DEFAULT '0.00',
  `ppQstart` float(8,2) NOT NULL DEFAULT '0.00',
  `ppQmin` float(8,2) NOT NULL DEFAULT '0.00',
  `ppQmax` float(8,2) NOT NULL DEFAULT '0.00',
  `ppNimp` float(8,2) NOT NULL DEFAULT '0.00',
  `ppConstP` float(8,2) NOT NULL DEFAULT '0.00',
  `ppConstT` float(8,2) NOT NULL DEFAULT '0.00',
  KEY `idpx` (`idpx`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `prefix_reports`
--

CREATE TABLE `prefix_reports` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `session_time` datetime NOT NULL,
  `prefix0` char(3) NOT NULL DEFAULT '0',
  `prefix1_3` char(8) NOT NULL DEFAULT '0',
  `prefix4_5` char(5) NOT NULL DEFAULT '0',
  `prefix8` char(3) NOT NULL DEFAULT '0',
  `prefix9_12` char(10) NOT NULL DEFAULT '0',
  `prefix13` char(3) NOT NULL DEFAULT '0',
  `prefix14` char(3) NOT NULL DEFAULT '0',
  `prefix15_22` char(16) NOT NULL DEFAULT '0',
  `prefix23_26` char(10) NOT NULL DEFAULT '0',
  `prefix31` char(3) NOT NULL DEFAULT '0',
  `crc_packet` char(4) NOT NULL DEFAULT '0',
  `crc_packet_ok` char(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `dt` (`session_time`),
  KEY `device_idx` (`prefix9_12`,`prefix13`,`prefix14`,`prefix8`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `receipt_requests`
--

CREATE TABLE `receipt_requests` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `serNUM` char(8) NOT NULL DEFAULT '0',
  `mfDEV` char(2) NOT NULL DEFAULT '0',
  `typeDEV` char(2) NOT NULL DEFAULT '0',
  `chNUM` char(3) NOT NULL DEFAULT '0',
  `CodeOp` char(2) NOT NULL DEFAULT '0',
  `Data` char(32) NOT NULL DEFAULT '0',
  `Status` char(1) NOT NULL DEFAULT '0',
  `ts` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `ts` (`ts`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `script_reports`
--

CREATE TABLE `script_reports` (
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `serNUM` char(8) NOT NULL DEFAULT '0',
  `mfDEV` char(2) NOT NULL DEFAULT '0',
  `typeDEV` char(2) NOT NULL DEFAULT '0',
  `chNUM` char(3) NOT NULL DEFAULT '0',
  `NumSim` char(1) NOT NULL DEFAULT '0',
  `EnabSim` char(1) NOT NULL DEFAULT '0',
  `EnabDoubSim` char(1) NOT NULL DEFAULT '0',
  `EnServSim` char(1) NOT NULL DEFAULT '0',
  `SetTmServSim` char(2) NOT NULL DEFAULT '0',
  `EnEmailSim` char(1) NOT NULL DEFAULT '0',
  `SetTmEmailSim` char(2) NOT NULL DEFAULT '0',
  `ContypeSim` char(16) NOT NULL DEFAULT '0',
  `APNSim` char(32) NOT NULL DEFAULT '0',
  `USERSim` char(16) NOT NULL DEFAULT '0',
  `PWDSim` char(16) NOT NULL DEFAULT '0',
  `IPadrSim` char(32) NOT NULL DEFAULT '0',
  `PortSim` char(5) NOT NULL DEFAULT '0',
  `SMTPSim` char(32) NOT NULL DEFAULT '0',
  `NameSim` char(32) NOT NULL DEFAULT '0',
  `PassSim` char(16) NOT NULL DEFAULT '0',
  `FromSim` char(32) NOT NULL DEFAULT '0',
  `FrNaSim` char(16) NOT NULL DEFAULT '0',
  `RecipSim` char(32) NOT NULL DEFAULT '0',
  `RecNaSim` char(16) NOT NULL DEFAULT '0',
  `CopySim` char(32) NOT NULL DEFAULT '0',
  `CopyNaSim` char(16) NOT NULL DEFAULT '0',
  `TopicSim` char(32) NOT NULL DEFAULT '0',
  `TMSIM` char(96) NOT NULL DEFAULT '0',
  PRIMARY KEY (`idpx`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `script_requests`
--

CREATE TABLE `script_requests` (
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `NumSim` tinyint(1) NOT NULL DEFAULT '0',
  `EnabSim` tinyint(1) NOT NULL DEFAULT '0',
  `EnabDoubSim` tinyint(1) NOT NULL DEFAULT '0',
  `EnServSim` tinyint(1) NOT NULL DEFAULT '0',
  `SetTmServSim` tinyint(2) NOT NULL DEFAULT '0',
  `EnEmailSim` tinyint(1) NOT NULL DEFAULT '0',
  `SetTmEmailSim` tinyint(2) NOT NULL DEFAULT '0',
  `ContypeSim` char(16) NOT NULL DEFAULT '0',
  `APNSim` char(32) NOT NULL DEFAULT '0',
  `USERSim` char(16) NOT NULL DEFAULT '0',
  `PWDSim` char(16) NOT NULL DEFAULT '0',
  `IPadrSim` char(32) NOT NULL DEFAULT '0',
  `PortSim` smallint(5) NOT NULL DEFAULT '0',
  `SMTPSim` char(32) NOT NULL DEFAULT '0',
  `NameSim` char(32) NOT NULL DEFAULT '0',
  `PassSim` char(16) NOT NULL DEFAULT '0',
  `FromSim` char(32) NOT NULL DEFAULT '0',
  `FrNaSim` char(16) NOT NULL DEFAULT '0',
  `RecipSim` char(32) NOT NULL DEFAULT '0',
  `RecNaSim` char(16) NOT NULL DEFAULT '0',
  `CopySim` char(32) NOT NULL DEFAULT '0',
  `CopyNaSim` char(16) NOT NULL DEFAULT '0',
  `TopicSim` char(32) NOT NULL DEFAULT '0',
  `TMSIM` char(96) NOT NULL DEFAULT '0',
  PRIMARY KEY (`idpx`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `telemetry_reports`
--

CREATE TABLE `telemetry_reports` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idpx` int(10) unsigned NOT NULL DEFAULT '0',
  `serNUM` char(10) NOT NULL DEFAULT '0',
  `mfDEV` char(3) NOT NULL DEFAULT '0',
  `typeDEV` char(3) NOT NULL DEFAULT '0',
  `chNUM` char(3) NOT NULL DEFAULT '0',
  `Clock_iMod` datetime NOT NULL,
  `SetClock` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `SetUTC` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `Clock_Corr` datetime NOT NULL,
  `GSMsignal` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `Vcontroller` float(9,2) unsigned NOT NULL DEFAULT '0.00',
  `Vext_power` float(9,2) unsigned NOT NULL DEFAULT '0.00',
  `Vaccumulator` float(9,2) NOT NULL DEFAULT '0.00',
  `Temp_iMod` float(9,2) NOT NULL DEFAULT '0.00',
  `Battery_life` float(9,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dump completed
