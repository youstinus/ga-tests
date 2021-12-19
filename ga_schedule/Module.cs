using System;

namespace ga_schedule
{
    public class Module
    {
        private readonly int _moduleId;
        private readonly string _moduleCode;
        private readonly string _module;
        private readonly int[] _professorIds;

        /**
         * Initialize new Module
         * 
         * @param moduleId
         * @param moduleCode
         * @param module
         * @param professorIds
         */
        public Module(int moduleId, string moduleCode, string module, int[] professorIds)
        {
            _moduleId = moduleId;
            _moduleCode = moduleCode;
            _module = module;
            _professorIds = professorIds;
        }

        /**
         * Get moduleId
         * 
         * @return moduleId
         */
        public int GetModuleId()
        {
            return _moduleId;
        }

        /**
         * Get module code
         * 
         * @return moduleCode
         */
        public string GetModuleCode()
        {
            return _moduleCode;
        }

        /**
         * Get module name
         * 
         * @return moduleName
         */
        public string GetModuleName()
        {
            return _module;
        }

        /**
         * Get random professor Id
         * 
         * @return professorId
         */
        public int GetRandomProfessorId()
        {
            var rnd = new Random();
            var professorId = _professorIds[(int)(_professorIds.Length * rnd.NextDouble())];
            return professorId;
        }
    }

}
