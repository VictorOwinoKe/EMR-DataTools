using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace EMRTools
{
    public partial class frmStandardQueries : Form
    {
        DataTable dt;

        public frmStandardQueries()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            string sQuery = "";

            if (cboQueries.SelectedIndex == 1)
            {
                sQuery = @"DROP TABLE IF EXISTS kenyaemr_etl.temp_all_vls;
                    DROP TABLE IF EXISTS kenyaemr_etl.temp_all_cd4;
                    DROP TABLE IF EXISTS kenyaemr_etl.temp_art_regimens;

                    create table kenyaemr_etl.temp_all_vls
                    select a.person_id as Patient_id, cast(a.obs_datetime as date) as vl_date, a.value_numeric as vl, count(*) as row_no from
                    (select person_id, obs_datetime, value_numeric from openmrs.obs where concept_id=856
                    union
                    select person_id, obs_datetime, 0 from openmrs.obs where concept_id=1305) a
                    inner join 
                    (select person_id, obs_datetime, value_numeric from openmrs.obs where concept_id=856
                    union
                    select person_id, obs_datetime, 0 from openmrs.obs where concept_id=1305) b 
                    on a.person_id=b.person_id and a.obs_datetime <= b.obs_datetime
                    group by a.person_id, a.obs_datetime, a.value_numeric;

                    alter table kenyaemr_etl.temp_all_vls add pkid int auto_increment primary key;
                    CREATE  INDEX ix_patient_id ON kenyaemr_etl.temp_all_vls(Patient_id);
                    CREATE  INDEX ix_row_no ON kenyaemr_etl.temp_all_vls(row_no);

                    create table kenyaemr_etl.temp_all_cd4
                    select a.person_id as Patient_id, a.obs_datetime as cd4_date, a.value_numeric as cd4_result, count(*) as row_no 
                    from
                    (select person_id, obs_datetime, value_numeric from openmrs.obs where concept_id=5497) a
                    inner join 
                    (select person_id, obs_datetime, value_numeric from openmrs.obs where concept_id=5497) b
                    on a.person_id=b.person_id and a.obs_datetime >= b.obs_datetime
                    group by a.person_id, a.obs_datetime, a.value_numeric;

                    alter table kenyaemr_etl.temp_all_cd4 add pkid_cd4 int auto_increment primary key;
                    CREATE  INDEX ix_patient_id_cd4 ON kenyaemr_etl.temp_all_cd4(Patient_id);
                    CREATE  INDEX ix_row_no_cd4 ON kenyaemr_etl.temp_all_cd4(row_no);

                    create table kenyaemr_etl.temp_art_regimens
                    SELECT a.patient_id
                    , b.encounter_datetime as date_created
                    , f.concept_id
                    , e.value_coded
                    , case when e.value_coded=792 then 'D4T/3TC/NVP'
	                    when e.value_coded=817 then 'AZT/3TC/ABC'
	                    when e.value_coded=1652 then 'AZT/3TC/NVP'
	                    when e.value_coded=160104 then 'D4T/3TC/EFV'
	                    when e.value_coded=160124 then 'AZT/3TC/EFV'
	                    when e.value_coded=162199 then 'ABC/3TC/NVP'
	                    when e.value_coded=162200 then 'ABC/3TC/LPV/r'
	                    when e.value_coded=162201 then 'TDF/3TC/LPV/r'
	                    when e.value_coded=162559 then 'ABC/DDI/LPV/r'
	                    when e.value_coded=162560 then 'D4T/3TC/LPV/r'
	                    when e.value_coded=162561 then 'AZT/3TC/LPV/r'
	                    when e.value_coded=162562 then 'TDF/ABC/LPV/r'
	                    when e.value_coded=162563 then 'ABC/3TC/EFV'
	                    when e.value_coded=162565 then 'TDF/3TC/NVP'
	                    when e.value_coded=164505 then 'TDF/3TC/EFV'
	                    when e.value_coded=164511 then 'AZT/3TC/ATV/r'
	                    when e.value_coded=164512 then 'TDF/3TC/ATV/r'
	                    when e.value_coded=164968 then 'AZT/3TC/DTG'
	                    when e.value_coded=164969 then 'TDF/3TC/DTG'
	                    when e.value_coded=164970 then 'ABC/3TC/DTG'
	                    when e.value_coded=164971 then 'TDF/3TC/AZT'
	                    when e.value_coded=164972 then 'AZT/TDF/3TC/LPV/r'
	                    when e.value_coded=164973 then 'ETR/RAL/DRV/RTV'
	                    when e.value_coded=164974 then 'ETR/TDF/3TC/LPV/r'
	                    when e.value_coded=164975 then 'D4T/3TC/ABC'
	                    when e.value_coded=164976 then 'ABC/TDF/3TC/LPV/r'
	                    when e.value_coded=165357 then 'ABC/3TC/ATV/r'
	                    when e.value_coded=165369 then 'TDF/3TC/DTG/DRV/r'
	                    when e.value_coded=165370 then 'TDF/3TC/RAL/DRV/r'
	                    when e.value_coded=165371 then 'TDF/3TC/DTG/EFV/DRV/r'
	                    when e.value_coded=165372 then 'ABC/3TC/RAL'
	                    when e.value_coded=165373 then 'AZT/3TC/RAL/DRV/r'
	                    when e.value_coded=165374 then 'ABC/3TC/RAL/DRV/r'
	                    when e.value_coded=165375 then 'RAL/3TC/DRV/RTV'
	                    when e.value_coded=165376 then 'RAL/3TC/DRV/RTV/AZT'
	                    when e.value_coded=165377 then 'RAL/3TC/DRV/RTV/ABC'
	                    when e.value_coded=165378 then 'ETV/3TC/DRV/RTV'
	                    when e.value_coded=165379 then 'RAL/3TC/DRV/RTV/TDF'
                        else 'OTHER'
	                    end as FieldValue
                    FROM openmrs.patient a
                    inner join openmrs.encounter b on a.patient_id=b.patient_id
                    inner join openmrs.encounter_type c on b.encounter_type = c.encounter_type_id
                    inner join openmrs.form d on b.form_id = d.form_id
                    inner join openmrs.obs e on b.encounter_id = e.encounter_id
                    inner join openmrs.concept f on e.concept_id = f.concept_id
                    inner join openmrs.concept_description h on f.concept_id = h.concept_id
                    inner join openmrs.concept_datatype i on f.datatype_id = i.concept_datatype_id
                    where d.name='Drug Regimen Editor'
                    and f.concept_id=1193;

                    alter table kenyaemr_etl.temp_art_regimens add pkid int auto_increment primary key;
                    CREATE  INDEX ix_patient_id ON kenyaemr_etl.temp_art_regimens(Patient_id);
                    CREATE  INDEX ix_date_created ON kenyaemr_etl.temp_art_regimens(date_created);

                    select a.patient_id
                    , a.unique_patient_no
                    , a.patient_clinic_number
                    , concat(ifnull(a.given_name,''), ' ', ifnull(a.middle_name,''), ' ', ifnull(a.family_name,'')) as patient_name 
                    , a.phone_number
                    , a.DOB
                    , round(TIMESTAMPDIFF(month, a.DOB, CURDATE())/12.0, 1) AS age
                    , a.Gender
                    , j.location
                    , j.ward
                    , j.sub_location
                    , j.village
                    , a.education_level
                    , a.marital_status
                    , a.next_of_kin
                    , a.next_of_kin_phone
                    , a.next_of_kin_relationship
                    , c.name_of_treatment_supporter
                    , c.treatment_supporter_telephone
                    , c.hiv_test_date
                    , c.date_first_enrolled_in_care
                    , c.Enrollment_Date
                    , case when f.pmtct_date_enrolled is not null then 'Yes' end as Enrolled_in_pmtct
                    , f.pmtct_date_enrolled
                    , f.pmtct_date_completed
                    , case when i.otz_date_enrolled is not null then 'Yes' end as Enrolled_in_otz
                    , i.otz_date_enrolled
                    , i.otz_date_completed
                    , case when k.ovc_date_enrolled is not null then 'Yes' end as Enrolled_in_ovc
                    , k.ovc_date_enrolled
                    , k.ovc_date_completed

                    , (select fieldvalue from kenyaemr_etl.temp_art_regimens x where x.patient_id=a.patient_id order by date_created limit 1) as Start_regimen
                    , (select date_created from kenyaemr_etl.temp_art_regimens x where x.patient_id=a.patient_id order by date_created limit 1) as Start_regimen_date
                    , (select fieldvalue from kenyaemr_etl.temp_art_regimens x where x.patient_id=a.patient_id order by date_created desc limit 1) as current_regimen
                    , (select date_created from kenyaemr_etl.temp_art_regimens x where x.patient_id=a.patient_id order by date_created desc limit 1) as current_regimen_date

                    , cd4.cd4_result as baseline_cd4
                    , cd4.cd4_date as baseline_cd4_date
                    , case when v1.vl_date < l.vl_order_date then l.vl_order_date 
	                    else null end as Pending_VL_order_date
                    , v1.vl as VL_latest
                    , v1.vl_date as VL_latest_date
                    , v2.vl as VL2
                    , v2.vl_date as VL2_date
                    , v3.vl as VL3
                    , v3.vl_date as VL3_date
                    , v4.vl as VL4
                    , v4.vl_date as VL4_date

                    , (select arv_adherence from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and length(arv_adherence)>1 order by visit_date desc limit 1) as arv_adherence
                    , (select resulting_tb_status from kenyaemr_datatools.tb_screening x where x.patient_id=a.patient_id order by visit_date desc limit 1) as tb_screening_result
                    , (select visit_date from kenyaemr_datatools.tb_screening x where x.patient_id=a.patient_id order by visit_date desc limit 1) as tb_screening_date
                    , (select on_anti_tb_drugs from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and length(on_anti_tb_drugs)>1 order by visit_date desc limit 1) as on_anti_tb_drugs
                    , h.tb_date_enrolled
                    , h.tb_date_completed
                    , (select ever_on_ipt from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and length(ever_on_ipt)>1 order by visit_date desc limit 1) as ever_on_ipt
                    , (select on_ipt from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and length(on_ipt)>1 order by visit_date desc limit 1) as on_ipt
                    , g.ipt_date_enrolled
                    , g.ipt_date_completed

                    , b.Last_Visit_date
                    , b.Appointment_date
                    , TIMESTAMPDIFF(day, b.Last_Visit_date, b.Appointment_date) as duration_in_days
                    , case when TIMESTAMPDIFF(DAY, b.Appointment_date, CURDATE()) <= 30 then 'Active' else 'Inactive' end as ART_Status
                    , case when TIMESTAMPDIFF(DAY, b.Appointment_date, CURDATE()) > 0 then TIMESTAMPDIFF(DAY, b.Appointment_date, CURDATE()) end as days_missed

                    , (select stability from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and length(stability)>1 order by visit_date desc limit 1) as stable
                    , (select differentiated_care from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and length(differentiated_care)>1 order by visit_date desc limit 1) as dc_model

                    , (select cacx_screening from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and length(cacx_screening)>1 order by visit_date desc limit 1) as cacx_screening

                    , case when b.Last_Visit_date < e.Discontinuation_date then e.Discontinuation_date end as Discontinuation_date
                    , case when b.Last_Visit_date < e.Discontinuation_date then e.discontinuation_reason end as discontinuation_reason

                    from (select patient_id, min(visit_date) as Enrollment_Date, min(date_first_enrolled_in_care) as date_first_enrolled_in_care
	                    , max(name_of_treatment_supporter) as name_of_treatment_supporter
	                    , max(treatment_supporter_telephone) as treatment_supporter_telephone
	                    , max(date_confirmed_hiv_positive) as hiv_test_date
	                    from kenyaemr_etl.etl_hiv_enrollment group by patient_id) c
                    inner join 
	                    (select patient_id, max(visit_date) as Last_Visit_date,  
	                    case when max(next_appointment_date) <= max(visit_date) then null else max(next_appointment_date) end as Appointment_date
	                    from kenyaemr_etl.etl_patient_hiv_followup x group by x.patient_id) b on c.patient_id=b.patient_id
                    inner join
	                    kenyaemr_etl.etl_patient_demographics a on a.patient_id=c.patient_id
                    left join
	                    (select patient_id, max(visit_date) as discontinuation_date, max(discontinuation_reason) as discontinuation_reason
	                    from kenyaemr_datatools.patient_program_discontinuation where program_name='HIV' group by patient_id) e on a.patient_id=e.patient_id
                    left join 
	                    (select patient_id, max(date_enrolled) as pmtct_date_enrolled, max(date_completed) as pmtct_date_completed
                        from kenyaemr_etl.etl_patient_program x 
	                    where program='MCH-Mother Services' and date_completed is null group by patient_id) f on a.patient_id=f.patient_id
                    left join
	                    (select patient_id, max(date_enrolled) as ipt_date_enrolled, max(date_completed) as ipt_date_completed
                        from kenyaemr_etl.etl_patient_program x 
	                    where program='IPT' and date_completed is null group by patient_id) g on a.patient_id=g.patient_id
                    left join
	                    (select patient_id, max(date_enrolled) as tb_date_enrolled, max(date_completed) as tb_date_completed
                        from kenyaemr_etl.etl_patient_program x 
	                    where program='tb' and date_completed is null group by patient_id) h on a.patient_id=h.patient_id
                    left join
	                    (select patient_id, max(date_enrolled) as otz_date_enrolled, max(date_completed) as otz_date_completed
                        from kenyaemr_etl.etl_patient_program x 
	                    where program='otz' and date_completed is null group by patient_id) i on a.patient_id=i.patient_id
                    left join
	                    (select patient_id, max(location) as location, max(ward) as ward, max(sub_location) as sub_location, max(village) as village 
	                    from kenyaemr_datatools.person_address group by patient_id) j on a.patient_id=j.patient_id
                    left join
	                    (select patient_id, max(date_enrolled) as ovc_date_enrolled, max(date_completed) as ovc_date_completed
                        from kenyaemr_etl.etl_patient_program x 
	                    where program='ovc' and date_completed is null group by patient_id) k on a.patient_id=k.patient_id 
                    left join
	                    (select b.patient_id, cast(max(encounter_datetime) as date) as vl_order_date from openmrs.orders a
	                    inner join openmrs.encounter b on a.encounter_id=b.encounter_id
	                    where a.concept_id in (856, 1305) group by b.patient_id) l on a.patient_id = l.patient_id
                    left join temp_all_vls v1 on a.patient_id = v1.patient_id and v1.row_no	= 1
                    left join temp_all_vls v2 on a.patient_id = v2.patient_id and v2.row_no	= 2
                    left join temp_all_vls v3 on a.patient_id = v3.patient_id and v3.row_no	= 3
                    left join temp_all_vls v4 on a.patient_id = v4.patient_id and v4.row_no	= 4
                    left join temp_all_cd4 cd4 on a.patient_id = cd4.patient_id and cd4.row_no = 1
                    where c.Enrollment_Date <= CURDATE()
                ";

                sQuery = sQuery.Replace("CURDATE()", "'" + dateTimePicker2.Text + @"'");
            }
            else if (cboQueries.SelectedIndex == 2)
            {
                sQuery = @"select * from
                (
	                select a.patient_id
                    , a.unique_patient_no
                    , concat(ifnull(a.given_name,''), ' ', ifnull(a.middle_name,''), ' ', ifnull(a.family_name,'')) as patient_name 
	                , coalesce(f.ServiceArea, 'ART') as Program
	                , TIMESTAMPDIFF(YEAR, a.DOB, '" + dateTimePicker2.Text + @"') AS age
	                , a.Gender
	                , a.phone_number
	                , '' as Residence
	                , c.Enrollment_Date
	                , a.next_of_kin
	                , a.next_of_kin_phone
	                , a.next_of_kin_relationship
	                , c.date_first_enrolled_in_care
	                , c.name_of_treatment_supporter
	                , c.treatment_supporter_telephone
                    , b.Last_Visit_date
	                , b.Appointment_date
	                , TIMESTAMPDIFF(DAY, b.Appointment_date, '" + dateTimePicker2.Text + @"') as days_missed
	                , d.tracing_type
	                , d.tracing_outcome
	                , d.attempt_number
                    , e.visit_date as Discontinuation_date
	                from kenyaemr_etl.etl_patient_demographics a
	                inner join 
	                (select patient_id, max(visit_date) as Last_Visit_date,  
                    case when max(next_appointment_date) <= max(visit_date) then null else max(next_appointment_date) end as Appointment_date
	                from kenyaemr_etl.etl_patient_hiv_followup x group by x.patient_id) b on a.patient_id=b.patient_id
	                left join
	                (select patient_id, min(visit_date) as Enrollment_Date, min(date_first_enrolled_in_care) as date_first_enrolled_in_care
                    , max(name_of_treatment_supporter) as name_of_treatment_supporter, 
	                max(treatment_supporter_telephone) as treatment_supporter_telephone
                    from kenyaemr_etl.etl_hiv_enrollment group by patient_id) c on a.patient_id=c.patient_id
	                left join
	                (select x.patient_id, max(x.tracing_type) as tracing_type, max(x.tracing_outcome) as tracing_outcome, max(x.attempt_number) as attempt_number 
	                from kenyaemr_etl.etl_ccc_defaulter_tracing x group by x.patient_id) d on a.patient_id=d.patient_id
                    left join
                    (select patient_id, visit_date, discontinuation_reason 
                    from kenyaemr_etl.etl_patient_program_discontinuation where program_name='ccc') e on a.patient_id=e.patient_id
                    left join 
                    (select patient_id, 'PMTCT' as ServiceArea, max(date_enrolled) as date_enrolled from kenyaemr_etl.etl_patient_program x 
	                where program='MCH-Mother Services' and date_completed is null group by patient_id) f on a.patient_id=f.patient_id
                )
                a where (a.days_missed >= 31 or a.days_missed is null)
                and a.Discontinuation_date is null
                and a.unique_patient_no is not null
                and a.last_visit_date >= '" + dateTimePicker1.Text + "'";
            }
            else if (cboQueries.SelectedIndex == 3)
            {
                sQuery = @"DROP TABLE IF EXISTS kenyaemr_etl.temp_art_regimens;

                    create table kenyaemr_etl.temp_art_regimens
                    SELECT a.patient_id
                    , b.encounter_datetime as date_created
                    , f.concept_id
                    , e.value_coded
                    , case when e.value_coded=792 then 'AF3A CF3A'
	                    when e.value_coded=817 then 'AF1C CF2C'
	                    when e.value_coded=1652 then 'AF1A CF1A'
	                    when e.value_coded=160104 then 'AF3B CF3B'
	                    when e.value_coded=160124 then 'AF1B CF1B'
	                    when e.value_coded=162199 then 'AF4A CF2A'
	                    when e.value_coded=162200 then 'AS3A CS2A'
	                    when e.value_coded=162201 then 'AS2A CG4A'
	                    when e.value_coded=162559 then 'AS3B AS3B'
	                    when e.value_coded=162560 then 'AS4A CS3A'
	                    when e.value_coded=162561 then 'AS1A CS1A'
	                    when e.value_coded=162562 then 'AF5X CF5X'
	                    when e.value_coded=162563 then 'AF4B CF2B'
	                    when e.value_coded=162565 then 'AF2A CF4A'
	                    when e.value_coded=164505 then 'AF2B CF4B'
	                    when e.value_coded=164511 then 'AS1B AS1B'
	                    when e.value_coded=164512 then 'AS2C AS2C'
	                    when e.value_coded=164968 then 'AF1D AF1D'
	                    when e.value_coded=164969 then 'AF2E CF4E'
	                    when e.value_coded=164970 then 'AF4C CF2G'
	                    when e.value_coded=164971 then 'AF2C AF2C'
	                    when e.value_coded=164972 then 'AS6X CS4X'
	                    when e.value_coded=164973 then 'AS6X CS4X'
	                    when e.value_coded=164974 then 'AS6X CS4X'
	                    when e.value_coded=164975 then 'AF3C AF3C'
	                    when e.value_coded=164976 then 'AS6X CS4X'
	                    when e.value_coded=165357 then 'AS3C AS3C'
	                    when e.value_coded=165369 then 'AS6X CS4X'
	                    when e.value_coded=165370 then 'AS6X CS4X'
	                    when e.value_coded=165371 then 'AS6X CS4X'
	                    when e.value_coded=165372 then 'CF2F CF2F'
	                    when e.value_coded=165373 then 'AS6X CS4X'
	                    when e.value_coded=165374 then 'CS2D CS2D'
	                    when e.value_coded=165375 then 'AS6X CS4X'
	                    when e.value_coded=165376 then 'AS6X CS4X'
	                    when e.value_coded=165377 then 'AS6X CS4X'
	                    when e.value_coded=165378 then 'AS6X CS4X'
	                    when e.value_coded=165379 then 'AS6X CS4X'
                        else 'AF5X CF5X'
	                    end as FieldValue
                    FROM openmrs.patient a
                    inner join openmrs.encounter b on a.patient_id=b.patient_id
                    inner join openmrs.encounter_type c on b.encounter_type = c.encounter_type_id
                    inner join openmrs.form d on b.form_id = d.form_id
                    inner join openmrs.obs e on b.encounter_id = e.encounter_id
                    inner join openmrs.concept f on e.concept_id = f.concept_id
                    inner join openmrs.concept_description h on f.concept_id = h.concept_id
                    inner join openmrs.concept_datatype i on f.datatype_id = i.concept_datatype_id
                    where d.name='Drug Regimen Editor'
                    and f.concept_id=1193;

                    alter table kenyaemr_etl.temp_art_regimens add pkid int auto_increment primary key;
                    CREATE  INDEX ix_patient_id ON kenyaemr_etl.temp_art_regimens(Patient_id);
                    CREATE  INDEX ix_date_created ON kenyaemr_etl.temp_art_regimens(date_created);

                    select coalesce(a.unique_patient_no, a.patient_clinic_number) as ccc_number
                    , a.given_name as first_name
                    , a.middle_name as other_name
                    , a.family_name as last_name
                    , DATE_FORMAT(a.dob, '%Y-%m-%d') as date_of_birth
                    , TIMESTAMPDIFF(YEAR, a.DOB, CURDATE()) AS age
                    , case when a.Gender='M' then 'Male' else 'Female' end as Gender
                    , (select weight from kenyaemr_etl.etl_patient_triage x where x.patient_id=a.patient_id and weight>0 order by date_created desc limit 1) as current_weight
                    , (select height from kenyaemr_etl.etl_patient_triage x where x.patient_id=a.patient_id and height>0 order by date_created desc limit 1) as current_height
                    , DATE_FORMAT(b.date_enrolled, '%Y-%m-%d') as date_enrolled
                    , 'outpatient' as patient_source
                    , case when e.patient_id is not null then 'ART'
	                    when f.patient_id is not null then 'PrEP'
                        when g.patient_id is not null then 'PMTCT'
                        when h.patient_id is not null then 'HEI'
                        else '' end as service_Type
                    , (select case when TIMESTAMPDIFF(YEAR, a.DOB, CURDATE())>=15 then SUBSTRING(fieldvalue, 1, 4) else SUBSTRING(fieldvalue, 6, 4) end
	                    from kenyaemr_etl.temp_art_regimens x where x.patient_id=a.patient_id order by date_created limit 1) as Start_regimen
                    , (select DATE_FORMAT(date_created, '%Y-%m-%d') from kenyaemr_etl.temp_art_regimens x where x.patient_id=a.patient_id order by date_created limit 1) as Start_regimen_date
                    , case when i.discontinuation_date > c.last_visit_date then i.discontinuation_reason
	                    else 'active' end as current_status
                    , (select case when TIMESTAMPDIFF(YEAR, a.DOB, CURDATE())>=15 then SUBSTRING(fieldvalue, 1, 4) else SUBSTRING(fieldvalue, 6, 4) end
	                    from kenyaemr_etl.temp_art_regimens x where x.patient_id=a.patient_id order by date_created desc limit 1) as current_regimen
                    , DATE_FORMAT(c.last_visit_date, '%Y-%m-%d') as last_visit_date
                    , DATE_FORMAT(c.Appointment_date, '%Y-%m-%d') as nextappointment
                    , TIMESTAMPDIFF(day, last_visit_date, c.Appointment_date) as days_to_nextappointment
                    , DATE_FORMAT(c.Appointment_date, '%Y-%m-%d') as clinicalappointment
                    , (select weight from kenyaemr_etl.etl_patient_triage x where x.patient_id=a.patient_id and weight>0 order by date_created limit 1) as start_weight
                    , (select height from kenyaemr_etl.etl_patient_triage x where x.patient_id=a.patient_id and height>0 order by date_created limit 1) as start_height
                    , DATE_FORMAT(d.hivTestDate, '%Y-%m-%d') as hivTestDate
                    , '' as transfer_from
                    , '' as prophylaxis
                    , '' isoniazid_start_date	
                    , '' isoniazid_end_date	
                    , '' rifap_isoniazid_start_date	
                    , '' rifap_isoniazid_end_date	
                    , '' differentiated_care_status
                    from kenyaemr_etl.etl_patient_demographics a
                    inner join
                    (select patient_id, min(date_enrolled) as date_enrolled from kenyaemr_etl.etl_patient_program x 
                    group by patient_id) b on a.patient_id=b.patient_id
                    left join
                    (select patient_id, max(visit_date) as last_visit_date, max(next_appointment_date) as Appointment_date 
                    from kenyaemr_etl.etl_patient_hiv_followup group by patient_id) c on a.patient_id=c.patient_id
                    left join
                    (select patient_id, min(date_first_enrolled_in_care) as date_enrolled, max(date_confirmed_hiv_positive) as hivTestDate
                    from kenyaemr_etl.etl_hiv_enrollment group by patient_id) d on a.patient_id=d.patient_id
                    left join
                    (select patient_id, min(date_enrolled) as date_enrolled from kenyaemr_etl.etl_patient_program x 
                    where x.program='hiv' group by patient_id) e on a.patient_id = e.patient_id
                    left join
                    (select patient_id, min(date_enrolled) as date_enrolled from kenyaemr_etl.etl_patient_program x 
                    where x.program='prep' group by patient_id) f on a.patient_id = f.patient_id
                    left join
                    (select patient_id, min(date_enrolled) as date_enrolled from kenyaemr_etl.etl_patient_program x 
                    where x.program='MCH-Mother Services' group by patient_id) g on a.patient_id = g.patient_id
                    left join
                    (select patient_id, min(date_enrolled) as date_enrolled from kenyaemr_etl.etl_patient_program x 
                    where x.program='MCH-Child Services' group by patient_id) h on a.patient_id = h.patient_id
                    left join
                    (select patient_id, max(visit_date) as discontinuation_date, 
                    case when max(discontinuation_reason) like '%transfer%' then 'Transfer out'
	                    when max(discontinuation_reason) like '%died%' then 'Deceased'
                        when max(discontinuation_reason) like '%lost%' then 'Lost to follow-up'
                        else 'Lost to follow-up' end as discontinuation_reason
                    from kenyaemr_datatools.patient_program_discontinuation where program_name='HIV' group by patient_id) i on a.patient_id=i.patient_id
                ";
            }
            else if (cboQueries.SelectedIndex == 4)
            {
                sQuery = @"DROP TABLE IF EXISTS kenyaemr_etl.temp_all_vls;
                    DROP TABLE IF EXISTS kenyaemr_etl.temp_all_cd4;
                    DROP TABLE IF EXISTS kenyaemr_etl.temp_art_regimens;

                    create table kenyaemr_etl.temp_all_vls
                    select a.person_id as Patient_id, cast(a.obs_datetime as date) as vl_date, a.value_numeric as vl, count(*) as row_no from
                    (select person_id, obs_datetime, value_numeric from openmrs.obs where concept_id=856
                    union
                    select person_id, obs_datetime, 0 from openmrs.obs where concept_id=1305) a
                    inner join
                    (select person_id, obs_datetime, value_numeric from openmrs.obs where concept_id=856
                    union
                    select person_id, obs_datetime, 0 from openmrs.obs where concept_id=1305) b
                    on a.person_id=b.person_id and a.obs_datetime <= b.obs_datetime
                    group by a.person_id, a.obs_datetime, a.value_numeric;

                    alter table kenyaemr_etl.temp_all_vls add pkid int auto_increment primary key;
                    CREATE  INDEX ix_patient_id ON kenyaemr_etl.temp_all_vls(Patient_id);
                    CREATE  INDEX ix_row_no ON kenyaemr_etl.temp_all_vls(row_no);

                    create table kenyaemr_etl.temp_all_cd4
                    select a.person_id as Patient_id, a.obs_datetime as cd4_date, a.value_numeric as cd4_result, count(*) as row_no
                    from
                    (select person_id, obs_datetime, value_numeric from openmrs.obs where concept_id=5497) a
                    inner join
                    (select person_id, obs_datetime, value_numeric from openmrs.obs where concept_id=5497) b
                    on a.person_id=b.person_id and a.obs_datetime >= b.obs_datetime
                    group by a.person_id, a.obs_datetime, a.value_numeric;

                    alter table kenyaemr_etl.temp_all_cd4 add pkid_cd4 int auto_increment primary key;
                    CREATE  INDEX ix_patient_id_cd4 ON kenyaemr_etl.temp_all_cd4(Patient_id);
                    CREATE  INDEX ix_row_no_cd4 ON kenyaemr_etl.temp_all_cd4(row_no);

                    create table kenyaemr_etl.temp_art_regimens
                    SELECT a.patient_id
                    , b.encounter_datetime as date_created
                    , f.concept_id
                    , e.value_coded
                    , case when e.value_coded=792 then 'D4T/3TC/NVP'
                    when e.value_coded=817 then 'AZT/3TC/ABC'
                    when e.value_coded=1652 then 'AZT/3TC/NVP'
                    when e.value_coded=160104 then 'D4T/3TC/EFV'
                    when e.value_coded=160124 then 'AZT/3TC/EFV'
                    when e.value_coded=162199 then 'ABC/3TC/NVP'
                    when e.value_coded=162200 then 'ABC/3TC/LPV/r'
                    when e.value_coded=162201 then 'TDF/3TC/LPV/r'
                    when e.value_coded=162559 then 'ABC/DDI/LPV/r'
                    when e.value_coded=162560 then 'D4T/3TC/LPV/r'
                    when e.value_coded=162561 then 'AZT/3TC/LPV/r'
                    when e.value_coded=162562 then 'TDF/ABC/LPV/r'
                    when e.value_coded=162563 then 'ABC/3TC/EFV'
                    when e.value_coded=162565 then 'TDF/3TC/NVP'
                    when e.value_coded=164505 then 'TDF/3TC/EFV'
                    when e.value_coded=164511 then 'AZT/3TC/ATV/r'
                    when e.value_coded=164512 then 'TDF/3TC/ATV/r'
                    when e.value_coded=164968 then 'AZT/3TC/DTG'
                    when e.value_coded=164969 then 'TDF/3TC/DTG'
                    when e.value_coded=164970 then 'ABC/3TC/DTG'
                    when e.value_coded=164971 then 'TDF/3TC/AZT'
                    when e.value_coded=164972 then 'AZT/TDF/3TC/LPV/r'
                    when e.value_coded=164973 then 'ETR/RAL/DRV/RTV'
                    when e.value_coded=164974 then 'ETR/TDF/3TC/LPV/r'
                    when e.value_coded=164975 then 'D4T/3TC/ABC'
                    when e.value_coded=164976 then 'ABC/TDF/3TC/LPV/r'
                    when e.value_coded=165357 then 'ABC/3TC/ATV/r'
                    when e.value_coded=165369 then 'TDF/3TC/DTG/DRV/r'
                    when e.value_coded=165370 then 'TDF/3TC/RAL/DRV/r'
                    when e.value_coded=165371 then 'TDF/3TC/DTG/EFV/DRV/r'
                    when e.value_coded=165372 then 'ABC/3TC/RAL'
                    when e.value_coded=165373 then 'AZT/3TC/RAL/DRV/r'
                    when e.value_coded=165374 then 'ABC/3TC/RAL/DRV/r'
                    when e.value_coded=165375 then 'RAL/3TC/DRV/RTV'
                    when e.value_coded=165376 then 'RAL/3TC/DRV/RTV/AZT'
                    when e.value_coded=165377 then 'RAL/3TC/DRV/RTV/ABC'
                    when e.value_coded=165378 then 'ETV/3TC/DRV/RTV'
                    when e.value_coded=165379 then 'RAL/3TC/DRV/RTV/TDF'
                        else 'OTHER'
                    end as FieldValue
                    FROM openmrs.patient a
                    inner join openmrs.encounter b on a.patient_id=b.patient_id
                    inner join openmrs.encounter_type c on b.encounter_type = c.encounter_type_id
                    inner join openmrs.form d on b.form_id = d.form_id
                    inner join openmrs.obs e on b.encounter_id = e.encounter_id
                    inner join openmrs.concept f on e.concept_id = f.concept_id
                    inner join openmrs.concept_description h on f.concept_id = h.concept_id
                    inner join openmrs.concept_datatype i on f.datatype_id = i.concept_datatype_id
                    where d.name='Drug Regimen Editor'
                    and f.concept_id=1193;

                    alter table kenyaemr_etl.temp_art_regimens add pkid int auto_increment primary key;
                    CREATE  INDEX ix_patient_id ON kenyaemr_etl.temp_art_regimens(Patient_id);
                    CREATE  INDEX ix_date_created ON kenyaemr_etl.temp_art_regimens(date_created);

                    select * from
                    (
                    select a.patient_id
                    , a.unique_patient_no
                    , a.patient_clinic_number
                    , concat(ifnull(a.given_name,''), ' ', ifnull(a.middle_name,''), ' ', ifnull(a.family_name,'')) as patient_name
                    , a.phone_number
                    , a.DOB
                    , round(TIMESTAMPDIFF(month, a.DOB, CURDATE())/12.0, 1) AS age
                    , a.Gender
                    , j.location
                    , j.ward
                    , j.sub_location
                    , j.village
                    , a.education_level
                    , a.marital_status
                    , a.next_of_kin
                    , a.next_of_kin_phone
                    , a.next_of_kin_relationship
                    , c.name_of_treatment_supporter
                    , c.treatment_supporter_telephone
                    , c.hiv_test_date
                    , c.date_first_enrolled_in_care
                    , c.Enrollment_Date
                    , case when f.pmtct_date_enrolled is not null then 'Yes' end as Enrolled_in_pmtct
                    , f.pmtct_date_enrolled
                    , f.pmtct_date_completed
                    , case when i.otz_date_enrolled is not null then 'Yes' end as Enrolled_in_otz
                    , i.otz_date_enrolled
                    , i.otz_date_completed
                    , case when k.ovc_date_enrolled is not null then 'Yes' end as Enrolled_in_ovc
                    , k.ovc_date_enrolled
                    , k.ovc_date_completed

                    , (select fieldvalue from kenyaemr_etl.temp_art_regimens x where x.patient_id=a.patient_id order by date_created limit 1) as Start_regimen
                    , (select date_created from kenyaemr_etl.temp_art_regimens x where x.patient_id=a.patient_id order by date_created limit 1) as Start_regimen_date
                    , (select fieldvalue from kenyaemr_etl.temp_art_regimens x where x.patient_id=a.patient_id order by date_created desc limit 1) as current_regimen
                    , (select date_created from kenyaemr_etl.temp_art_regimens x where x.patient_id=a.patient_id order by date_created desc limit 1) as current_regimen_date

                    , (select x.weight from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and x.weight>0 order by visit_date desc limit 1) as current_weight
                    , (select x.height from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and x.height>0 order by visit_date desc limit 1) as current_height

                    , cd4.cd4_result as baseline_cd4
                    , cd4.cd4_date as baseline_cd4_date
                    , case when v1.vl_date < l.vl_order_date then l.vl_order_date
                    else null end as Pending_VL_order_date
                    , v1.vl as VL_latest
                    , v1.vl_date as VL_latest_date
                    , v2.vl as VL2
                    , v2.vl_date as VL2_date
                    , v3.vl as VL3
                    , v3.vl_date as VL3_date
                    , v4.vl as VL4
                    , v4.vl_date as VL4_date

                    , (select arv_adherence from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and length(arv_adherence)>1 order by visit_date desc limit 1) as arv_adherence
                    , (select resulting_tb_status from kenyaemr_datatools.tb_screening x where x.patient_id=a.patient_id order by visit_date desc limit 1) as tb_screening_result
                    , (select visit_date from kenyaemr_datatools.tb_screening x where x.patient_id=a.patient_id order by visit_date desc limit 1) as tb_screening_date
                    , (select on_anti_tb_drugs from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and length(on_anti_tb_drugs)>1 order by visit_date desc limit 1) as on_anti_tb_drugs
                    , h.tb_date_enrolled
                    , h.tb_date_completed
                    , (select ever_on_ipt from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and length(ever_on_ipt)>1 order by visit_date desc limit 1) as ever_on_ipt
                    , (select on_ipt from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and length(on_ipt)>1 order by visit_date desc limit 1) as on_ipt
                    , g.ipt_date_enrolled
                    , g.ipt_date_completed

                    , b.Last_Visit_date
                    , b.Appointment_date
                    , TIMESTAMPDIFF(day, b.Last_Visit_date, b.Appointment_date) as duration_in_days
                    , case when TIMESTAMPDIFF(DAY, b.Appointment_date, CURDATE()) <= 30 then 'Active' else 'Inactive' end as ART_Status
                    , case when TIMESTAMPDIFF(DAY, b.Appointment_date, CURDATE()) > 0 then TIMESTAMPDIFF(DAY, b.Appointment_date, CURDATE()) end as days_missed

                    , (select stability from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and length(stability)>1 order by visit_date desc limit 1) as stable
                    , (select differentiated_care from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and length(differentiated_care)>1 order by visit_date desc limit 1) as dc_model

                    , (select cacx_screening from kenyaemr_datatools.hiv_followup x where x.patient_id=a.patient_id and length(cacx_screening)>1 order by visit_date desc limit 1) as cacx_screening

                    , case when b.Last_Visit_date < e.Discontinuation_date then e.Discontinuation_date end as Discontinuation_date
                    , case when b.Last_Visit_date < e.Discontinuation_date then e.discontinuation_reason end as discontinuation_reason

                    from (select patient_id, min(visit_date) as Enrollment_Date, min(date_first_enrolled_in_care) as date_first_enrolled_in_care
                    , max(name_of_treatment_supporter) as name_of_treatment_supporter
                    , max(treatment_supporter_telephone) as treatment_supporter_telephone
                    , max(date_confirmed_hiv_positive) as hiv_test_date
                    from kenyaemr_etl.etl_hiv_enrollment group by patient_id) c
                    inner join
                    (select patient_id, max(visit_date) as Last_Visit_date,  
                    case when max(next_appointment_date) <= max(visit_date) then null else max(next_appointment_date) end as Appointment_date
                    from kenyaemr_etl.etl_patient_hiv_followup x group by x.patient_id) b on c.patient_id=b.patient_id
                    inner join
                    kenyaemr_etl.etl_patient_demographics a on a.patient_id=c.patient_id
                    left join
                    (select patient_id, max(visit_date) as discontinuation_date, max(discontinuation_reason) as discontinuation_reason
                    from kenyaemr_datatools.patient_program_discontinuation where program_name='HIV' group by patient_id) e on a.patient_id=e.patient_id
                    left join
                    (select patient_id, max(date_enrolled) as pmtct_date_enrolled, max(date_completed) as pmtct_date_completed
                        from kenyaemr_etl.etl_patient_program x
                    where program='MCH-Mother Services' and date_completed is null group by patient_id) f on a.patient_id=f.patient_id
                    left join
                    (select patient_id, max(date_enrolled) as ipt_date_enrolled, max(date_completed) as ipt_date_completed
                        from kenyaemr_etl.etl_patient_program x
                    where program='IPT' and date_completed is null group by patient_id) g on a.patient_id=g.patient_id
                    left join
                    (select patient_id, max(date_enrolled) as tb_date_enrolled, max(date_completed) as tb_date_completed
                        from kenyaemr_etl.etl_patient_program x
                    where program='tb' and date_completed is null group by patient_id) h on a.patient_id=h.patient_id
                    left join
                    (select patient_id, max(date_enrolled) as otz_date_enrolled, max(date_completed) as otz_date_completed
                        from kenyaemr_etl.etl_patient_program x
                    where program='otz' and date_completed is null group by patient_id) i on a.patient_id=i.patient_id
                    left join
                    (select patient_id, max(location) as location, max(ward) as ward, max(sub_location) as sub_location, max(village) as village
                    from kenyaemr_datatools.person_address group by patient_id) j on a.patient_id=j.patient_id
                    left join
                    (select patient_id, max(date_enrolled) as ovc_date_enrolled, max(date_completed) as ovc_date_completed
                        from kenyaemr_etl.etl_patient_program x
                    where program='ovc' and date_completed is null group by patient_id) k on a.patient_id=k.patient_id
                    left join
                    (select b.patient_id, cast(max(encounter_datetime) as date) as vl_order_date from openmrs.orders a
                    inner join openmrs.encounter b on a.encounter_id=b.encounter_id
                    where a.concept_id in (856, 1305) group by b.patient_id) l on a.patient_id = l.patient_id
                    left join kenyaemr_etl.temp_all_vls v1 on a.patient_id = v1.patient_id and v1.row_no= 1
                    left join kenyaemr_etl.temp_all_vls v2 on a.patient_id = v2.patient_id and v2.row_no= 2
                    left join kenyaemr_etl.temp_all_vls v3 on a.patient_id = v3.patient_id and v3.row_no = 3
                    left join kenyaemr_etl.temp_all_vls v4 on a.patient_id = v4.patient_id and v4.row_no = 4
                    left join kenyaemr_etl.temp_all_cd4 cd4 on a.patient_id = cd4.patient_id and cd4.row_no = 1
                    where c.Enrollment_Date <= CURDATE()
                    ) a where a.ART_Status='Active'
                    ";

                sQuery = sQuery.Replace("CURDATE()", "'" + dateTimePicker2.Text + @"'");
            }

            MySqlConnection conn = new MySqlConnection(UtilityFunctions.GetConnString());
            MySqlCommand command = new MySqlCommand(sQuery, conn);
            command.CommandTimeout = 0;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataSet dataset = new DataSet();

            try
            {
                adapter.Fill(dataset);
                dt = dataset.Tables[0];
                dataGridView1.DataSource = dt;
                lblRecords.Text = dt.Rows.Count.ToString() + " Records";
            }
            catch (Exception ex)
            {
                lblRecords.Text = "0 Records";
                MessageBox.Show(ex.Message, "SQL query error");
            }

            this.Cursor = Cursors.Default;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            ExcelFunctions func = new ExcelFunctions();
            if (cboQueries.SelectedIndex == 3)
            {
                func.ExportToExcel_mysqlFormat(dt);
            }
            else
            {
                func.ExportToExcel(dt);
            }

            this.Cursor = Cursors.Default;
        }

        private void frmStandardQueries_Load(object sender, EventArgs e)
        {
            cboQueries.SelectedIndex = 0;
        }

        private void cboQueries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboQueries.SelectedIndex == 1)//ART line list
            {
                dateTimePicker1.Visible = false;
                dateTimePicker2.Visible = true;
                lblStartDate.Visible = false;
                lblEnddate.Visible = true;
            }
            else if (cboQueries.SelectedIndex == 2) //ltfu
            {
                dateTimePicker1.Visible = true;
                dateTimePicker2.Visible = true;
                lblStartDate.Visible = true;
                lblEnddate.Visible = true;
            }
            else if (cboQueries.SelectedIndex == 3) //ADT patient list
            {
                dateTimePicker1.Visible = false;
                dateTimePicker2.Visible = false;
                lblStartDate.Visible = false;
                lblEnddate.Visible = false;
            }
            else if (cboQueries.SelectedIndex == 4)//Active on ART line list
            {
                dateTimePicker1.Visible = false;
                dateTimePicker2.Visible = true;
                lblStartDate.Visible = false;
                lblEnddate.Visible = true;
            }
        }
    }
}
